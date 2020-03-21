using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

class TimerPBR
{
    private System.Timers.Timer timer;

    public delegate void method(object sender, ElapsedEventArgs e);

    public TimerPBR(double minutes, method myMethod)
    {
        // Create new time
        timer = new System.Timers.Timer(minutes * 60000);

        // Pass a method
        timer.Elapsed += new System.Timers.ElapsedEventHandler(myMethod);

        timer.AutoReset = true;
        timer.Enabled = true;
    }

    // Start created timer
    public void StartTimer()
    {
        // Save start, to repeat methods in case of error every 10 minutes
        while (true)
        {
            try
            {
                timer.Start();
                break; // success!
            }
            catch
            {
                Console.WriteLine("I GOT AN ERROR INSIDE TIMER, TRYING TO REPEAT: " + DateTime.Now);
                Thread.Sleep(100000);
            }
        }
    }
}

