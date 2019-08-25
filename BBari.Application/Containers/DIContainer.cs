using AutoMapper;
using BBari.Application.AutoMapper;
using BBari.Application.Interfaces;
using BBari.Application.Services;
using BBari.EventBus.Events;
using BBari.EventBus.Interfaces;
using BBari.Utils.JsonExtention;
using Confluent.Kafka;
using Newtonsoft.Json.Linq;
using Ninject.Modules;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BBari.Application.Containers
{
    public class DIContainer : NinjectModule
    {
        public override void Load()
        {


            FileStream fileStream = new FileStream(Directory.GetCurrentDirectory() + "/appsettings.json", FileMode.Open);
            StreamReader reader = new StreamReader(fileStream);
            string line = reader.ReadToEnd();


            reader.Dispose();
            fileStream.Dispose();

            Bind<JsonHandle>().ToSelf().WithConstructorArgument(line);
            //Bind(typeof(JsonAdapter<>)).ToSelf();
            //Bind<Samurai>().ToSelf();
            //Bind(typeof(IMessageConsumer)).To(typeof(MessageConsumer)).InSingletonScope();

            Bind(typeof(IEventProducer<>)).To<EventProducer>().InTransientScope();

            //var consumer = new ConsumerBuilder<Ignore, string>(new ConsumerConfig
            //{
            //    GroupId = JObject.Parse(line)["ConsumerConfigure"]["GroupId"].Value<string>(),
            //    BootstrapServers = JObject.Parse(line)["ConsumerConfigure"]["BootstrapServers"].Value<string>(),
            //    AutoOffsetReset = (AutoOffsetReset)Enum.Parse(typeof(AutoOffsetReset), JObject.Parse(line)["ConsumerConfigure"]["AutoOffsetReset"].Value<string>()),
            //    EnableAutoCommit = JObject.Parse(line)["ConsumerConfigure"]["EnableAutoCommit"].Value<bool>()
            //}).Build();

            //Bind<IConsumer<Ignore, string>>().ToConstant(consumer).InSingletonScope();

            Bind(typeof(IEventConsumer<>)).To<EventConsumer>().InTransientScope();

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MessageMappingProfile());

            }).CreateMapper();

            Bind<IMapper>().ToConstant(mapperConfiguration).InSingletonScope();

            Bind<IAppMessageService>().To<AppMessageService>().InSingletonScope();

            // This teaches Ninject how to create automapper instances say if for instance
            // MyResolver has a constructor with a parameter that needs to be injected
            //Bind<IMapper>().ToMethod(ctx =>
            //     new Mapper(mapperConfiguration, type => ctx.Kernel.Get(type)));


            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .MinimumLevel.Debug()
               .WriteTo.ColoredConsole(
                   LogEventLevel.Verbose,
                   "{NewLine}{Timestamp:HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}")
                   .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();

            Bind<ILogger>().ToConstant(Log.Logger).InSingletonScope();

        }
    }
}
