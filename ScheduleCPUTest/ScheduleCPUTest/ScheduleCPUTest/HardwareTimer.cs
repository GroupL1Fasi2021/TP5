using System;
using System.Timers;

public class HardwareTimer : Timer
{	
	/// <summary>
	/// Counting the number of millisecs
	/// </summary>
    public long Clock = 0;

    /// <summary>
    /// The length of the timer interval in milliseconds
    /// </summary>
    public const long TickLength = 100;
	
	public HardwareTimer()
	{
		this.Interval = TickLength;
	}
	
	/// <summary> 
    /// Adds an ITimeTickReceiver to the list
    /// </summary>
	/// <param name="receiver">The ITimeTickReceiver to be added</param>
	public void AddTickReceiver(ITimeTickReceiver receiver)
	{
		if (receiver != null)
		{
            this.Elapsed += receiver.ReceiveTimeTick;
		}
	}

    /// <summary> 
    /// Starts the timer
    /// </summary>
    public void StartTimer()
    {
        this.Elapsed += this.IncreaseClock;
        this.Enabled = true;
    }

    /// <summary> 
    /// Stops the timer
    /// </summary>
    public void StopTimer()
    {
        this.Enabled = false;
    }

    /// <summary>
    /// Increase the clock according to the timer interval
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    private void IncreaseClock(object source, ElapsedEventArgs e)
    {
        Clock+=TickLength;
    }
}