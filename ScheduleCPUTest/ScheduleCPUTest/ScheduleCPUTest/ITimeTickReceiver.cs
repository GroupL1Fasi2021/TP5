using System;
using System.Timers;

/// <summary> 
/// Interface to be implemented by receivers of ticks of the HardwareTimer
/// </summary>
public interface ITimeTickReceiver
{
	/// <summary> 
    /// This method is called when a timertick occurs
    /// </summary>
	/// <param name="clock">number of current timertick</param>
    void ReceiveTimeTick(object source, ElapsedEventArgs e);
}