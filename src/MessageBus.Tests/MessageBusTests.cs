using NUnit.Framework;
using System.Collections.Generic;

namespace DDD
{
    [TestFixture]
    class MessageBusTests
    {
        [Test]
        public void PublishMessageA_SubscribedForMessageA_ReceivesMessageA()
        {
            var receivedMessages = new List<object>();
            bus.Subscribe<MessageA>(m => receivedMessages.Add(m));

            bus.Publish(new MessageA());

            Assert.That(receivedMessages, Has.Exactly(1).InstanceOf<MessageA>());
        }

        [SetUp]
        protected void SetUp()
        {
            bus = new MessageBus();
        }

        private IMessageBus bus;
    }

    internal class MessageA { }

    internal class MessageB { }
}
