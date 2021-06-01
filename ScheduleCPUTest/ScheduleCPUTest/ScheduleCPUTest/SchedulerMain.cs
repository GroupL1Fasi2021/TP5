using System;
using System.Threading;

/// <summary>
/// This program demonstrates how the scheduler operates.
/// This creates the CPU, scheduler and timer, and then the three example processes.
/// </summary>
public class SchedulerMain
{
    public static void Main(System.String[] args)
    {
        // Create virtual computer
        CPU singleCPU = new CPU();
        long tmd = 0;
        Scheduler CPUScheduler = new Scheduler(singleCPU);
        HardwareTimer timer = new HardwareTimer();
        // Add hardware components to hardware timer
        timer.AddTickReceiver(singleCPU);
        timer.AddTickReceiver(CPUScheduler);

        // Create process 1
        IProcess tp1 = new TestProcess("process 1", 10000);

        // Add process 1 to the system
        CPUScheduler.AddProcess(tp1);

        // Create process 2
        IProcess tp2 = new TestProcess("process 2", 8000);

        // Add process 2 to the system
        CPUScheduler.AddProcess(tp2);

        // Create process 3
        IProcess tp3 = new TestProcess("process 3", 2000);

        // Add process 3 to the system
        CPUScheduler.AddProcess(tp3);

        // Start hardware timer
        timer.StartTimer();
        while (!CPUScheduler.NoProcesses)
            Thread.Sleep(100);
        Console.WriteLine("\nAll processes finished");
        timer.StopTimer();
        Scheduler.AverageTATAndWT();
        // TODO print information after all processes are finished
        Console.ReadLine();
    }

}