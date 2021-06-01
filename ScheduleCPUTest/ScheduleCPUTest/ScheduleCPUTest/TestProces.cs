using System.Timers;
using System;

/// <summary> 
/// A process that will be executed by the CPU 
/// </summary>
public class TestProcess : IProcess
{
    private string name;
    private bool ready = false;
    private long cpuTimeNeeded; // Amount of CPU time needed to finish execution
    private long clock; // The current time in this simulator
    private long startTime = -1;
    private static long val2 = 0;
    private const int quantum = 2000;
    private static long val1 = 0;
    private Scheduler sch;

    /// <summary> 
    /// Creates a process with a name and the amount of timerticks needed to finish execution
    /// </summary>
    /// <param name="id">name of the process</param>
    /// <param name="processtime">amount of timerticks needed to finish execution</param>
    public TestProcess(string id, long processtime)
    {
        this.Name = id;
        this.cpuTimeNeeded = processtime;

        //this.sch = new Scheduler();

    }

    /// <summary>
    /// Set the startTime of this process
    /// </summary>
    public long StartTime
    {
        set
        {
            startTime = value;
        }
    }

    /// <summary> 
    /// The amount of CPU-time this process still needs
    /// </summary>
    public long CPUTimeNeeded
    {
        get
        {
            return this.cpuTimeNeeded;
        }

    }

    /// <summary> 
    /// Checks whether the process is finished
    /// </summary>
    public bool Ready
    {
        get
        {
            return this.ready;
        }
    }

    /// <summary>
    /// The name of this process
    /// </summary>
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            this.name = value;
        }
    }

    /// <summary> 
    /// Returns turnaround time of proces in timerticks between begin and end of the process
    /// Note: If the process did not finish yet, return 0
    /// </summary>
    public long TurnAroundTime
    {
        get
        {
            return Val1 - Val2;
        }
    }

    //
    public long Val2
    {
        get
        {

            return val2;
        }
        set
        {
            val2 = value;
        }
    }


    //
    public long Val1
    {
        get
        {

            return val1;
        }
        set
        {
            val1 = value;
        }
    }


    /// <summary> 
    /// Returns turnaround time of proces in timerticks between begin and end of the process
    /// Note: If the process did not finish yet, return 0
    /// </summary>
    public long WaitingTime
    {
        get
        {
            // TODO waiting time
            return Val2 - Val1;
        }
    }

    /// <summary> 
    /// Returns the initial number of timerticks needed to finish execution
    /// This is the initial value of timecount
    /// </summary>
    public long InitialCPUTimeNeeded
    {
        get
        {
            // TODO initial CPU time needed
            return CPUTimeNeeded;
        }
    }

    /// <summary> 
    /// Decrements timecount
    /// </summary>
    /// <returns> true if timecount == 0, false otherwise
    /// </returns>
    private bool decreaseCPUTimeNeeded()
    {
        lock (this)
        {
            cpuTimeNeeded -= HardwareTimer.TickLength;
            return this.cpuTimeNeeded == 0;
        }
    }

    /// <summary>
    /// This method is called when a timertick occurs
    /// Decrements timecount
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public void ReceiveTimeTick(object source, ElapsedEventArgs e)
    {
        if (!this.ready)
        {
            this.ready = this.decreaseCPUTimeNeeded();
        }
        clock = ((HardwareTimer)source).Clock;
    }

}