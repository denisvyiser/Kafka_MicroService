using BBari.Application.Commands;
using BBari.Application.Containers;
using BBari.Application.Interfaces;
using BBari.Application.Services;
using BBari.Utils.JsonExtention;
using Ninject;
using Serilog;
using System;
using System.Threading;

namespace BBari.Services
{
    class Program
    {

        static ConsoleKeyInfo key;
        static Thread[] thread;
        static JsonHandle jsonHandle;
        static IAppMessageService appMessage;
        static IKernel kernel;
        static int ProducerTimer;
        static int ConsumerTimer;
        static string identificador;
        static ILogger logger;
        static void Main(string[] args)
        {
            kernel = new StandardKernel(new DIContainer());
            jsonHandle = kernel.Get<JsonHandle>();

            ProducerTimer = Int32.Parse(jsonHandle.JsonKey("ProducerTimer"));
            ConsumerTimer = Int32.Parse(jsonHandle.JsonKey("ConsumerTimer"));
            identificador = jsonHandle.JsonKey("Identificador");

            appMessage = kernel.Get<AppMessageService>();

            thread = new Thread[] { new Thread(new ThreadStart(ThreadProducer)) { Priority = ThreadPriority.Highest }, new Thread(new ThreadStart(ThreadConsumer)) { Priority = ThreadPriority.Lowest } };

            thread[0].Start();

            thread[1].Start();

            key = new ConsoleKeyInfo();
            key = Console.ReadKey();

            

            //Console.WriteLine("Hello World!");


        }

        public static void ThreadProducer()
        {
            while (key.Key != ConsoleKey.Enter)
            {
                

                var message = new NewMessage(Guid.NewGuid(), identificador, "Hello Word", DateTime.Now);
                appMessage.MessageProducerService(message);

                Thread.Sleep(ProducerTimer);
            }
        }

        public static void ThreadConsumer()
        {
            while (key.Key != ConsoleKey.Enter)
            {
                appMessage.MessageConsumererService();
                Thread.Sleep(ProducerTimer);
            }
        }


        }
    }
