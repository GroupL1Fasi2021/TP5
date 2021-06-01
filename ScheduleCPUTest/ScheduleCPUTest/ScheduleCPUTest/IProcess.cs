using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

public interface IProcess : ITimeTickReceiver
{
    string Name { get; set; }
    bool Ready { get; }
    long TurnAroundTime { get; }
    long WaitingTime { get; }
    long InitialCPUTimeNeeded { get; }
    long StartTime { set; }
}
