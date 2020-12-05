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
        public void PublishMessageA_SubscribedForMessageAButThenHandlerReleased_NothingHappens()
        {
            int counter = 0;
            new MessageAHandler(bus, m => counter++);
            GC.Collect(); // Handler should have been disposed now...

            bus.Publish(new MessageAA());

            Assert.That(counter, Is.EqualTo(0));
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

    class MessageA { }

    class MessageB { }

    class MessageAA : MessageA { }

    class MessageAHandler
    {
        private readonly Action<MessageA> callback;

        public MessageAHandler(IMessageBus bus, Action<MessageA> callback)
        {
            this.callback = callback;
            bus.Subscribe<MessageA>(OnMessageA);
        }

        private void OnMessageA(MessageA e)
        {
            callback(e);
        }
    }
}
