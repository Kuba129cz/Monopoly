using System;
using System.Diagnostics;
using System.IO;

namespace Monopoly.Communication
{
    static class Latency
    {
        static Stopwatch stopWatch;
        public static void SendRequest(string sendingCode)
        {
            stopWatch = new Stopwatch();
            stopWatch.Restart();
            stopWatch.Start();
            using (StreamWriter sw = new StreamWriter(@"Latency.txt", true))
            {
                sw.Write("{0}; {1}; ", DateTime.Now, sendingCode);
                sw.Flush();
            }
        }
        public static void ReceiveRequest(string receiveCode)
        {
            if (stopWatch != null)
            {
                stopWatch.Stop();
                using (StreamWriter sw = new StreamWriter(@"Latency.txt", true))
                {
                    sw.Write("{0}; time: {1}", receiveCode, stopWatch.ElapsedMilliseconds);
                    sw.WriteLine();
                    sw.Flush();
                }
                stopWatch.Reset();
            }
        }
    }
}
