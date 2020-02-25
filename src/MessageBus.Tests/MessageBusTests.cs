using NUnit.Framework;
using System;

namespace DDD
{
    [TestFixture]
    class MessageBusTests
    {
        [Test]
        public void PublishMessageA_SubscribedForMessageA_ReceivesMessageA()
        {
            int counter = 0;
            bus.Subscribe<MessageA>(m => counter++);

            bus.Publish(new MessageA());

            Assert.That(counter, Is.EqualTo(1));
        }

        [Test]
        public void PublishMessageA_TwoHandlersSubscribedForTwoMessageTypes_OnlyMessageAReceivesMessage()
        {
            int counter = 0;
            bus.Subscribe<MessageA>(m => counter++);
            bus.Subscribe<MessageB>(m => counter++);

            bus.Publish(new MessageA());

            Assert.That(counter, Is.EqualTo(1));
        }

        [Test]
        public void PublishMessageAA_SubscribedForMessageA_ReceivesMessageC()
        {
            int counter = 0;
            bus.Subscribe<MessageA>(m => counter++);

            bus.Publish(new MessageAA());

            Assert.That(counter, Is.EqualTo(1));
        }

        [Test]
        public void PublishMessageA_NoSubscribtionForMessageA_NothingHappens()
        {
            Assert.DoesNotThrow(() => bus.Publish(new MessageAA()));
        }

        [Test]
        public void PublishMessageA_SubscriberForMessageAThrowsException_NothingHappens()
        {
            bus.Subscribe<MessageA>(m => throw new Exception());
            Assert.DoesNotThrow(() => bus.Publish(new MessageAA()));
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

    internal class MessageAA : MessageA { }
}
