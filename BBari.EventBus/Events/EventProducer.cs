using BBari.Domain.Entities;
using BBari.EventBus.Interfaces;
using BBari.Utils.JsonExtention;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBari.EventBus.Events
{
    public class EventProducer : IEventProducer<Message>
    {

        IProducer<Null, string> producer;
        string topic;

        public EventProducer(IProducer<Null, string> producer, string topic)
        {

            this.producer = producer;
            this.topic = topic;

           
        }

        public void Send(Message data)
        {
            
            try
            {
                string jsonData = JsonAdapter<Message>.Serialize(data);

                Action<DeliveryReport<Null, string>> handler = r => 
            Console.WriteLine(!r.Error.IsError
                ? $"Delivered message to {r.TopicPartitionOffset}"
                : $"Delivery Error: {r.Error.Reason}");

                producer.Produce(topic, new Message<Null, string> { Value = jsonData }, handler);

                
                producer.Flush(TimeSpan.FromSeconds(10));
            
            }
            catch (ProduceException<Null, string> ex)
            {
                Console.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
