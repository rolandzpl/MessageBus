using NUnit.Framework;
using System.Collections.Generic;

namespace DDD
{
    [TestFixture]
    class MessageBusTests
    {
        [Test]
        public void x()
        {
            var receivedMessages = new List<object>();
            bus.Subscribe<MessageA>(m => receivedMessages.Add(m));

            Assert.That(receivedMessages, Has.Exactly(1).InstanceOf<object>());
        }

        [SetUp]
        protected void SetUp()
        {
            bus = new MessageBus();
        }

        private IMessageBus bus;
    }

    internal class MessageA
    {
    }
}
