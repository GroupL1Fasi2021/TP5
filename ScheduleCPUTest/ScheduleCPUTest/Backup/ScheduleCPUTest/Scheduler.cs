using System.Timers;

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
    private long clock=0; // the current time in this simulator

    public Scheduler(CPU cpu)
    {
        this.currentCPU = cpu;
        timeSlice = DEFAULT_TIME_SLICE;
        this.timeSliceCounter = 0;
        queue = new CircularProcesList();
        cpu.Scheduler = this;
    }

    public Scheduler(CPU cpu, int quantum)
        : this(cpu)
    {
        timeSlice = quantum;
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
    /// Receive ticks until timequantum has finished, then schedule new proces
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public void ReceiveTimeTick(object source, ElapsedEventArgs e)
    {
        this.clock = ((HardwareTimer)source).Clock;
        this.timeSliceCounter--;
        if (this.timeSliceCounter <= 0)
        {
            // Time slice has finished, therefor reschedule
            this.schedule();
            this.timeSliceCounter = this.timeSlice / 100;
        }
    }

    /// <summary> 
    /// The actual scheduling operation
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

                // add to queue if not ready
                if (!removedProcess.Ready)
                {
                    System.Console.Out.WriteLine(clock + "\tadding to queue " + removedProcess.Name);
                    this.queue.AddItem(removedProcess);
                }
                else
                {
                    System.Console.Out.WriteLine(clock + "\tFINISHED: " + removedProcess.Name);
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
            }
            // no current processes
            else
            {
                System.Console.Out.WriteLine(clock + "\tqueue empty");
            }
        }
    }
}