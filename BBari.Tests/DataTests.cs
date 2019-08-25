using BBari.Application.Commands;
using BBari.Application.Containers;
using BBari.Application.Interfaces;
using BBari.Application.Services;
using BBari.Utils.JsonExtention;
using Moq;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBari.Tests
{
    [TestFixture]
    public class DataTests
    {
        static JsonHandle jsonHandle;
        //static IAppMessageService appMessage;
        static IKernel kernel;
        static string identificador;

        [SetUp]
        public void SetUp()
        {

            kernel = new StandardKernel(new DIContainer());
            jsonHandle = kernel.Get<JsonHandle>();
            identificador = jsonHandle.JsonKey("Identificador");
            //appMessage = kernel.Get<AppMessageService>();

        }

        [Test]
        public void TestProducerService()
        {
            var message = new NewMessage(Guid.NewGuid(), identificador, "Hello Word", DateTime.Now);


            var mockApi = new Mock<IAppMessageService>();
            mockApi.Setup(svc => svc.MessageProducerService(message));
        }


        [Test]
        public void TestConsumereService()
        {
            var message = new NewMessage(Guid.NewGuid(), identificador, "Hello Word", DateTime.Now);


             var mockApi = new Mock<IAppMessageService>();
             mockApi.Setup(svc => svc.MessageConsumererService()).Returns(message);

            Assert.AreEqual(message.Data, message.Data);
        }

       
        [Test]

        public void TestProducerService2()
        {
            //Assert.AreEqual

        }
    }
}
