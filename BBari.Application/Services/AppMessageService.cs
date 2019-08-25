using AutoMapper;
using BBari.Application.Commands;
using BBari.Application.Containers;
using BBari.Application.Interfaces;
using BBari.Domain.Entities;
using BBari.EventBus.Events;
using BBari.EventBus.Interfaces;
using BBari.Utils.JsonExtention;
using Confluent.Kafka;
using Ninject;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBari.Application.Services
{
    public class AppMessageService : IAppMessageService
    {
        static ILogger logger;
        IMapper mapper;
        JsonHandle jsonHandle;
        IEventProducer<Message> eventProducer;
        IEventConsumer<Message> eventConsumer;
        IKernel kernel;

        public AppMessageService()
        {
            kernel = new StandardKernel(new DIContainer());
            jsonHandle = kernel.Get<JsonHandle>();
            mapper = kernel.Get<IMapper>();
            logger = kernel.Get<ILogger>();

        }

        public void MessageProducerService(NewMessage message)
        {
            try
            {
            var producer = new Ninject.Parameters.ConstructorArgument("producer", new ProducerBuilder<Null, string>(new ProducerConfig
            {
                BootstrapServers = jsonHandle.JsonKey("ConsumerConfigure.BootstrapServers")
            }).Build());

            //var producer = new Ninject.Parameters.ConstructorArgument("producer", kernel.Get<IProducer<Null,string>>());
            var topic = new Ninject.Parameters.ConstructorArgument("topic", jsonHandle.JsonKey("ProducerConfigure.Topic"));
            eventProducer = kernel.Get<EventProducer>(producer, topic);
            

           var newMessage = mapper.Map<NewMessage, Message>(message);

                logger.Information("Teste");
                eventProducer.Send(newMessage);
            }catch(Exception ex)
            {
                logger.Error(ex.ToString());
            }

        }


        public NewMessage MessageConsumererService()
        {
            
          
            var consumer = new Ninject.Parameters.ConstructorArgument("consumer", new ConsumerBuilder<Ignore, string>(new ConsumerConfig
            {
                //GroupId = JObject.Parse(line)["ConsumerConfigure"]["GroupId"].Value<string>(),
                GroupId = jsonHandle.JsonKey("ConsumerConfigure.GroupId"),
                BootstrapServers = jsonHandle.JsonKey("ConsumerConfigure.BootstrapServers"),
                AutoOffsetReset = (AutoOffsetReset)Enum.Parse(typeof(AutoOffsetReset), jsonHandle.JsonKey("ConsumerConfigure.AutoOffsetReset")),
                EnableAutoCommit = bool.Parse(jsonHandle.JsonKey("ConsumerConfigure.EnableAutoCommit"))
            }).Build());
            eventConsumer = kernel.Get<EventConsumer>(consumer);
            
            var newMessage = mapper.Map<Message, NewMessage>(eventConsumer.Handle());

            return newMessage;
        }

    }
}
