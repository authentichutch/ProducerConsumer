using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer.Console
{

    public class ObservingDataController : IDataController
    {
        private IConsumer _consumer;
        private IProducer _producer;
        private IObservable<string> _receiveObservable;

        public ObservingDataController(IProducer producer, IConsumer consumer)
        {
            _producer = producer;
            _consumer = consumer;
        }

        public async Task StartReceivingData()
        {
            await Task.Run(()=>ObserveReceiver());
        }


        private async Task ObserveReceiver()
        {
            _receiveObservable = Observable.Create<string>((observer) =>
            {
                string data = _producer.ReceiveData();

                if (!string.IsNullOrEmpty(data))
                {
                    observer.OnNext(data); //async
                }
                else
                {
                    System.Console.WriteLine("No Data to Process");
                }

                observer.OnCompleted();

                return Disposable.Empty;
            }).Repeat();

            _receiveObservable.ObserveOn(TaskPoolScheduler.Default).Subscribe((s) => { _consumer.ProcessData(s); });

        }

       
    }
}
