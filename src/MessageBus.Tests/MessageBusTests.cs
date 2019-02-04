using NUnit.Framework;

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
            bus.Subscribe<MessageAA>(m => counter++);

            bus.Publish(new MessageA());

            Assert.That(counter, Is.EqualTo(1));
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
