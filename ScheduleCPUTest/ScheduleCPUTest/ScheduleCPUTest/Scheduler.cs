using System.Timers;
using System;

/// <summary>
/// The scheduler
/// </summary>
public class Scheduler : ITimeTickReceiver
{
    private CPU currentCPU;
    private CircularProcesList queue;
    private long timeSlice;
    private long timeSliceCounter;
    private const int DEFAULT_TIME_SLICE = 2000; // default timeslice is 2 seconds
    private long clock = 0; // the current time in this simulator

    //private TestProcess tps;

    //
    private static long[] initialCPUTimeNeededTab = new long[] { 0, 0, 0 };
    private static long[] waitingTimeTab = new long[] { 0, 0, 0 };
    private static long[] processesTab = new long[] { 1, 2, 3 };
    private static long[] turnAroundTimeTab = new long[] { 0, 0, 0 };
    private static long[] lastTimeTab = new long[] { 0, 0, 0 };
    private static long[] firstTimeTab = new long[] { 0, 0, 0 };
    private static String[] firstNameProcessesTab = new String[] { "", "", "" };
    private static String[] lastNameProcessesTab = new String[] { "", "", "" };
    private static int noOfProcessesAdded = 0;
    private static int firstnumber = 0;
    private static int lastnumber = 0;

    public Scheduler(CPU cpu)
    {
        this.currentCPU = cpu;
        //this.timeSlice = timeSlice;
        this.timeSlice = DEFAULT_TIME_SLICE;
        this.timeSliceCounter = 0;
        queue = new CircularProcesList();
        cpu.Scheduler = this;

    }

    public Scheduler(CPU cpu, int quantum)
        : this(cpu)
    {
        this.timeSlice = quantum;
    }

    public long TimeSlice
    {
        get
        {
            return timeSlice;
        }
        set
        {
            this.timeSlice = Int32.MaxValue;
        }
    }

    /// <summary> 
    /// The average turnaround time of all processes that finished up to now
    /// </summary>
    public bool NoProcesses
    {
        get
        {
            lock (this)
            {
                return queue.Empty && !currentCPU.Busy;
            }
        }
    }

    /// <summary> 
    /// Introduces a new TestProcess that must be execeuted on the CPU
    /// </summary>
    /// <param name="t">The new TestProcess</param>
    public void AddProcess(IProcess t)
    {
        queue.AddItem(t);
        t.StartTime = clock;
        initialCPUTimeNeededTab[noOfProcessesAdded] = t.InitialCPUTimeNeeded;
        //System.Console.WriteLine(t.ToString());
        this.timeSlice = t.InitialCPUTimeNeeded;
        noOfProcessesAdded++;
    }


    // Another Method to find the waiting time
    // for all processes

    public static void TurnAroundTime()
    {

        TestProcess t = new TestProcess("tempo", 0);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (firstNameProcessesTab[i] == lastNameProcessesTab[j])
                {
                    //Console.WriteLine(tabOfFirstNameProcesses[i] +"=="+ tabOfLastNameProcesses[j]);
                    //
                    t.Val1 = lastTimeTab[j];
                    t.Val2 = firstTimeTab[i];
                    turnAroundTimeTab[i] = t.TurnAroundTime;
                    //
                }
            }
        }


    }
    public static void WaitingTime()
    {
        TestProcess t = new TestProcess("tempo", 0);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (firstNameProcessesTab[i] == lastNameProcessesTab[j])
                {
                    //Console.WriteLine(tabOfFirstNameProcesses[i] +"=="+ tabOfLastNameProcesses[j]);
                    //
                    t.Val2 = turnAroundTimeTab[i];
                    t.Val1 = initialCPUTimeNeededTab[i];
                    waitingTimeTab[i] = t.WaitingTime;
                }
            }
        }
    }

    // Function calculate average time

    public static void AverageTATAndWT()
    {
        long total_wt = 0;
        long total_tat = 0;
        TurnAroundTime();
        WaitingTime();
        for (int i = 0; i < noOfProcessesAdded; i++)
        {
            total_wt = total_wt + waitingTimeTab[i];
            total_tat = total_tat + turnAroundTimeTab[i];
        }

        // Show All processes
        Console.WriteLine("\n Processes\t|\t Initial CPUTimeNeeded\t|\t TurnAroundTime\t\t|\t Waiting Time\n");
        for (int i = 0; i < noOfProcessesAdded; i++)
        {
            Console.WriteLine(processesTab[i] + "\t\t|\t\t" + initialCPUTimeNeededTab[i] + "\t\t|\t\t" + turnAroundTimeTab[i] + "\t\t|\t\t" + waitingTimeTab[i]);
        }

        for (int i = 0; i < noOfProcessesAdded; i++)
        {
            Console.WriteLine("\n Process : Process" + processesTab[i] + "\nInitial CPUTimeNeeded: " + initialCPUTimeNeededTab[i] + "\nTurnAroundTime: " + turnAroundTimeTab[i] + "\nWaiting Time: " + waitingTimeTab[i] + "\n\t_ _ _\t_ _ _");

        }
        // Average turn around time
        Console.WriteLine("\nAverage Turn Around Time \t" + (float)total_tat / (float)noOfProcessesAdded);
        // Average waiting time
        Console.WriteLine("Average Waiting Time \t" + (float)total_wt / (float)noOfProcessesAdded);

    }


    /// <summary> 
    /// Signal to the scheduler that scheduling is needed in the next timertick
    /// </summary>
    public void schedulingNeeded()
    {
        this.timeSliceCounter = 0;
    }

    /// <summary>
    /// This method is called when a timertick occurs
    /// Receive ticks until time quantum has finished, then schedule new proces
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public void ReceiveTimeTick(object source, ElapsedEventArgs e)
    {
        this.clock = ((HardwareTimer)source).Clock;
        this.timeSliceCounter--;
        if (this.timeSliceCounter <= 0)
        {
            // Time slice has finished, there for reschedule
            this.schedule();
            this.timeSliceCounter = this.timeSlice / 100;
        }
    }

    /// <summary> 
    /// The actual scheduling operation For Round Robin
    /// </summary>
    public void schedule()
    {
        lock (this)
        {
            System.Console.Out.WriteLine(clock + "\t* * * Context Switch * * * ");
            IProcess current;

            // remove process from CPU and put in queue
            IProcess removedProcess = this.currentCPU.RemoveProcess();
            if (removedProcess != null)
            {
                System.Console.Out.WriteLine(clock + "\tremoving from CPU " + removedProcess.Name);

                // Receiver about the First time of process in the CPU
                // and the name of process
                if (firstnumber < 3)
                {
                    firstTimeTab[firstnumber] = clock - TimeSlice;
                    firstNameProcessesTab[firstnumber] = removedProcess.Name;
                    firstnumber = firstnumber + 1;
                }
                // add to queue if not ready
                if (!removedProcess.Ready)
                {
                    System.Console.Out.WriteLine(clock + "\tadding to queue " + removedProcess.Name);
                    this.queue.AddItem(removedProcess);
                }
                else
                {
                    // Receiver about the Last time of process in the CPU
                    // and the name of process

                    System.Console.Out.WriteLine(clock + "\tFINISHED: " + removedProcess.Name);
                    System.Console.Out.WriteLine(clock + "\t" + removedProcess.Name + " ends at " + clock + "ms");

                    if (lastnumber < 3)
                    {
                        lastTimeTab[lastnumber] = clock;
                        lastNameProcessesTab[lastnumber] = removedProcess.Name;
                        lastnumber = lastnumber + 1;
                    }

                }
            }

            // select new process for CPU
            current = queue.Next;

            // end of scheduling algorithm

            // start the selected process on the CPU (if any)
            if (current != null)
            {
                System.Console.Out.WriteLine(clock + "\tputting on CPU " + current.Name);
                this.currentCPU.SetProcess(current);
                //System.Console.Out.WriteLine("\n"+current.Name+" Brust Time :"+current.InitialCPUTimeNeeded);
                //TimeSlice = current.InitialCPUTimeNeeded;                
            }
            // no current processes
            else
            {
                System.Console.Out.WriteLine(clock + "\tqueue empty");
            }
        }
    }
}