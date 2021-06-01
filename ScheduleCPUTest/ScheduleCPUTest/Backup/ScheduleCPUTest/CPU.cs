using System;
using System.Timers;

/// <summary> 
/// This class represents the hardware CPU
/// </summary>
public class CPU : ITimeTickReceiver
{
    /// <summary>
    /// The Process currently running on the CPU
    /// </summary>
    private IProcess current = null;

    /// <summary>
    /// The Scheduler
    /// </summary>
    private Scheduler scheduler;

	/// <summary> 
    /// Make the scheduler known to the CPU
    /// </summary>
	/// <param name="scheduler"> The Scheduler</param>
	public Scheduler Scheduler
	{
		set
		{
			this.scheduler = value;
		}
	}

    /// <summary>
    /// Check whether the CPU is currently busy
    /// </summary>
    public bool Busy
    {
        get
        {
            return current != null;
        }
    }
    
    /// <summary> 
    /// Assigns a new TestProcess to run on the CPU<
    /// /summary>
    /// <param name="Process">The Process to be assigned to the CPU</param>
    public void SetProcess(IProcess Process)
	{
		this.current = Process;
	}

	/// <summary> 
    /// Removes the current process from the CPU
    /// </summary>
	/// <returns> The removed Process
	/// </returns>
	public IProcess RemoveProcess()
	{
		IProcess tmp = this.current;
		this.current = null;
		return tmp;
	}
	
    /// <summary>
    /// This methode will be called by the HardwareTimer after each timer tick
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public void ReceiveTimeTick(object source, ElapsedEventArgs e)
	{
		if (this.current != null)
		{
			this.current.ReceiveTimeTick(source,e);
			// warn scheduler if process has finished
			if (this.current.Ready)
			{
				this.scheduler.schedulingNeeded();
			}
		}
	}
}