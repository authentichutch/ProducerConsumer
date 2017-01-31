using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var main = MainObservable(args);
            main.Wait();

            //var main = MainAsync(args);
            //main.Wait();


            System.Console.WriteLine("Done");
            System.Console.ReadLine();
        }

        private async static Task MainObservable(string[] args)
        {
            BlockingProducer producer = new BlockingProducer(5, 10);
            BlockingConsumer consumer = new BlockingConsumer(10);

            ObservingDataController controller = new ObservingDataController(producer, consumer);
            await controller.StartReceivingData();

        }
        private static async Task MainAsync(string[] args)
        {
            BlockingProducer producer = new BlockingProducer(10, 10);
            BlockingConsumer consumer = new BlockingConsumer(10);

            ConcurrentQueue<string> buffer = new ConcurrentQueue<string>(); //unbounded buffer - block while empty

            QueueingDataController controller = new QueueingDataController(producer, consumer, buffer);
          
            await controller.StartReceivingData(); //n.b. one of these tasks appears to starve the other if prod/cons too unbalanced
            

            
        }
    }
}
