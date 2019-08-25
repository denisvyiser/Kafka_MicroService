using BBari.EventBus.Events;
using BBari.EventBus.Interfaces;
using BBari.Utils.JsonExtention;
using Confluent.Kafka;
using Newtonsoft.Json.Linq;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BBari.IoC
{
    public class DIContainer : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IEventProducer<>)).To<EventProducer>().InSingletonScope();
            Bind(typeof(IEventConsumer<>)).To<EventConsumer>().InSingletonScope();

            FileStream fileStream = new FileStream(Directory.GetCurrentDirectory() + "/appsettings.json", FileMode.Open);
            StreamReader reader = new StreamReader(fileStream);
            string line = reader.ReadToEnd();


            reader.Dispose();
            fileStream.Dispose();

            Bind<JsonHandle>().ToSelf().WithConstructorArgument(line);
            //Bind(typeof(JsonAdapter<>)).ToSelf();
            //Bind<Samurai>().ToSelf();
            //Bind(typeof(IMessageConsumer)).To(typeof(MessageConsumer)).InSingletonScope();

            var producer = new ProducerBuilder<Null, string>(new ProducerConfig {
                BootstrapServers = JObject.Parse(line)["BootstrapServers"].Value<string>()
            }).Build();
            Bind(typeof(IProducer<Null, string>)).ToSelf();

            var consumer = new ConsumerBuilder<Ignore, string>(new ConsumerConfig
            {
                GroupId = JObject.Parse(line)["GroupId"].Value<string>(),
                BootstrapServers = JObject.Parse(line)["BootstrapServers"].Value<string>(),
                AutoOffsetReset = JObject.Parse(line)["AutoOffsetReset"].Value<AutoOffsetReset>(),
                EnableAutoCommit = JObject.Parse(line)["EnableAutoCommit"].Value<bool>()
            }).Build();

            Bind(typeof(IConsumer<Ignore, string>)).ToSelf();
        }
    }
}
