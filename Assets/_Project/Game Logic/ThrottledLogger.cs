using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLogic
{
    public static class ThrottledLogger
    {

        private static int logThrottleAmountMS = 5000; //only log to one path every 5 seconds.

        private static Dictionary<string, DateTime> lastLogTimes = new Dictionary<string, DateTime>();
        public static void LogToFile(string path, string contents)
        {
            var currentTime = DateTime.Now;

            
            
            if (lastLogTimes.ContainsKey(path))
            {
                TimeSpan timeSinceLast = currentTime -lastLogTimes[path];
                if (timeSinceLast.TotalMilliseconds < logThrottleAmountMS)
                {
                    return;
                }
            }

            if (!lastLogTimes.ContainsKey(path))
            {
                lastLogTimes.Add(path, currentTime);
            }

            using (var sw = new StreamWriter("CustomLogs/"+ path + currentTime.ToFileTime()))
            {
                sw.WriteLine(contents);
            }

            Debug.Log("File write success!");

            lastLogTimes[path] = currentTime;


        }
    }
}
