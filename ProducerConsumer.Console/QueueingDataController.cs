using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.Console
{
    public class QueueingDataController: IDataController
    {
        private IProducerConsumerCollection<string> _buffer;
        private IConsumer _consumer;
        private IProducer _producer;

        private bool _receiving;
        

        public QueueingDataController(IProducer producer, IConsumer consumer, IProducerConsumerCollection<string> queue)
        {
            _producer = producer;
            _consumer = consumer;
            _buffer = queue;
        }
        public async Task StartReceivingData()
        {
            var receiveTask = StartReceiving();
            var processTask = StartProcessing();
            await Task.WhenAll(receiveTask, processTask); 
        }

        private async Task StartReceiving()
        {
            if (!_receiving)
            {
                _receiving = true; //TODO: race condition

                while (_receiving)
                {
                    bool result = await Task.Run(() => AddDataToReceiveBuffer());
                    if (!result)
                    {
                        System.Console.WriteLine("Failed to add item to queue");
                        //TODO: exception? retry? drop data?
                    }
                }
            }
        }


        private async Task StartProcessing()
        {
            while (_receiving)
            {
                var receivedData = await Task.Run(() => ConsumeDataFromBuffer());
                if (!receivedData)
                {
                    await Task.Delay(1000); //arbitrary delay - todo could do better
                }
            }

        }

        private bool ConsumeDataFromBuffer()
        {
            string payload;
            var gotData = _buffer.TryTake(out payload);

            if (gotData)
            {
                System.Console.WriteLine("Processing Data From Buffer");
                System.Console.WriteLine("Buffer: " + _buffer.Count);
                _consumer.ProcessData(payload);
            }
            else
            {
                System.Console.WriteLine("Empty Buffer");
            }
            return gotData;
        }

        private bool AddDataToReceiveBuffer()
        {
            var added = _buffer.TryAdd(_producer.ReceiveData());
            if (added)
            {
                System.Console.WriteLine("Added data received to buffer");
                System.Console.WriteLine("Buffer: " + _buffer.Count);
            }

            return added;
        }



    }
}
