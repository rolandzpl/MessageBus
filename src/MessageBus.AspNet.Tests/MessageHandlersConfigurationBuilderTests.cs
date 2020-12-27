using System;
using Moq;
using NUnit.Framework;

namespace DDD
{
    public class MessageHandlersConfigurationBuilderTests
    {
        [Test]
        public void Build_HandlerWasAdded_PublishedEventGetsHandledInSystem()
        {
            var builder = new MessageHandlersConfigurationBuilder(serviceProvider);
            builder.AddHandler<SampleMessage, SampleMessageHandler>();
            builder.Build();
            bus.Publish(new SampleMessage());
            Assert.That(handler.Count, Is.EqualTo(1));
        }

        [Test]
        public void Build_DelegateWasAdded_PublishedEventGetsHandledInSystem()
        {
            int counter = 0;
            var builder = new MessageHandlersConfigurationBuilder(serviceProvider);
            builder.AddHandler<SampleMessage>(m => counter++);
            builder.Build();
            bus.Publish(new SampleMessage());
            Assert.That(counter, Is.EqualTo(1));
        }

        [SetUp]
        protected void SetUp()
        {
            bus = new MessageBus();
            handler = new SampleMessageHandler();
            serviceProvider = Mock.Of<IServiceProvider>(_ =>
                _.GetService(typeof(IMessageBus)) == bus &&
                _.GetService(typeof(SampleMessageHandler)) == handler);
        }

        private IServiceProvider serviceProvider;
        private IMessageBus bus;
        private SampleMessageHandler handler;
    }

    class SampleMessage { }

    class SampleMessageHandler
    {
        public int Count { get; private set; }

        private void OnSampleMessage(SampleMessage msg)
        {
            Count++;
        }
    }
}