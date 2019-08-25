using BBari.Domain.Entities;
using BBari.EventBus.Interfaces;
using BBari.Utils.JsonExtention;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BBari.EventBus.Events
{
    public class EventConsumer : IEventConsumer<Message>
    {

        IConsumer<Ignore, string> consumer;

        public EventConsumer(IConsumer<Ignore, string> consumer)
        {
            this.consumer = consumer;
        }

        public Message Handle()
        {
            Message message = null;
            
                    try
                    {
                        consumer.Subscribe("hello-topic");

                        CancellationTokenSource cts = new CancellationTokenSource();
                        Console.CancelKeyPress += (_, e) =>
                        {
                            e.Cancel = true;
                            cts.Cancel();
                        };

                        var cr = consumer.Consume(cts.Token);

                    message = JsonAdapter<Message>.Deserialize(cr.Value);

                         
                    }
                    catch (ConsumeException ex)
                    {
                        Console.WriteLine($"Ocorreu um erro: {ex.Error.Reason}");
                    }
            return message;
        }
            
            
        
    }
}
