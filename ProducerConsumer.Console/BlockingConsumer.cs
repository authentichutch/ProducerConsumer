using System;
using System.Diagnostics;
using System.Threading;

namespace ProducerConsumer.Console
{
    /// <summary>
    /// Simlates Process data 
    /// </summary>
    public class BlockingConsumer : IConsumer
    {
        private int _maxWait;
        private Random _waitGenerator;

        public BlockingConsumer(int maxWait)
        {
            _maxWait = maxWait;
            _waitGenerator = new Random();
        }
        public void ProcessData(string data)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var busyTime = _waitGenerator.Next(0,_maxWait);
            Thread.Sleep(TimeSpan.FromSeconds(busyTime)); //simulate work

            sw.Stop();
            System.Console.WriteLine(string.Format("Processed {0} in {1} ",data,sw.Elapsed));
        }
    }
}
