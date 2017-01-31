using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer.Console
{
    /// <summary>
    /// Simulates receving data (strings) from a resource that blocks
    /// </summary>
    public class BlockingProducer : IProducer
    {
        private int _maxPayload;
        private int _maxWait;
        private Random _randomNumberGenerator;

        public BlockingProducer(int maxWait, int maxPayload)
        {
            _maxWait = maxWait;
            _maxPayload = maxPayload;
            _randomNumberGenerator = new Random();
        }
        public string ReceiveData()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
  
            string payload = GeneratePayload();

            var busyTime = _randomNumberGenerator.Next(0, _maxWait);
            Thread.Sleep(TimeSpan.FromSeconds(busyTime));

            sw.Stop();
            System.Console.WriteLine(string.Format("RX {0} characters:{1} - in {2}", payload.Length, payload, sw.Elapsed));

            return payload;
            //wait for random amount of time
        }

        private string GeneratePayload()
        {
            //generate random length string of random characters
            var payloadSize = _randomNumberGenerator.Next(0, _maxPayload);

            StringBuilder sb = new StringBuilder(payloadSize);
            for (int i = 0; i < payloadSize; i++)
            {
                sb.Append((char)_randomNumberGenerator.Next(0, 255));
            }

            return sb.ToString();
        }
    }
}
