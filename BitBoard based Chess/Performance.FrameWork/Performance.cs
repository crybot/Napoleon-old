using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Performance.FrameWork
{
    public static class PerformanceTester
    {
        public static Int64 TestPerformance(Action actionToPerform)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            actionToPerform.Invoke();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}
