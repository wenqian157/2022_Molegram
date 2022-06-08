using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class testEvent : MonoBehaviour
{
}

class Counter
{
    public event EventHandler ThresholdReached;
    //public delegate void ThresholdReachedEventHandler(object sender, ThresholdReachedEventArgs e);

    protected virtual void OnThresholdReached(EventArgs e)
    {
        EventHandler handler = ThresholdReached;
        handler?.Invoke(this, e);
    }

    // provide remaining implementation for the class
}

//public class ThresholdReachedEventArgs : EventArgs
//{
//    public int Threshold { get; set; }
//    public DateTime TimeReached { get; set; }
//}

class Program
{
    static void Main()
    {
        var c = new Counter();
        c.ThresholdReached += c_ThresholdReached;

        // provide remaining implementation for the class
    }

    static void c_ThresholdReached(object sender, EventArgs e)
    {
        Console.WriteLine("The threshold was reached.");
    }
}
