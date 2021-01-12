using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DDD
{
    public class MessageHandlersConfigurationBuilder
    {
        private readonly List<HandlerRegistrationBase> registrations = new List<HandlerRegistrationBase>();
        private readonly IServiceProvider serviceProvider;

        public MessageHandlersConfigurationBuilder(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public MessageHandlersConfigurationBuilder AddHandler<TMessage>(Action<TMessage> callback)
        {
            registrations.Add(new DelegatedHandlerRegistration<TMessage>(callback));
            return this;
        }

        public MessageHandlersConfigurationBuilder AddHandler<TMessage, TMessageHandler>()
        {
            registrations.Add(
                new HandlerRegistration(
                    serviceProvider: serviceProvider,
                    messageType: typeof(TMessage),
                    handlerType: typeof(TMessageHandler)));
            return this;
        }

        public void Build()
        {
            Trace.WriteLine($"Preparing for message sunbscription configuration. Found {registrations.Count} subscriptions.");
            var bus = serviceProvider.GetRequiredService<IMessageBus>();
            foreach (var r in registrations)
            {
                r.SubscribeHandler(bus);
            }
        }

        abstract class HandlerRegistrationBase
        {
            public abstract void SubscribeHandler(IMessageBus bus);
        }

        class HandlerRegistration : HandlerRegistrationBase
        {
            private readonly Type messageType;
            private readonly Type handlerType;
            private readonly IServiceProvider serviceProvider;

            public HandlerRegistration(Type messageType, Type handlerType, IServiceProvider serviceProvider)
            {
                this.messageType = messageType;
                this.handlerType = handlerType;
                this.serviceProvider = serviceProvider;
            }

            public override void SubscribeHandler(IMessageBus bus)
            {
                var handler = serviceProvider.GetRequiredService(handlerType);
                var methods = handlerType
                    .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Where(_ => HasOneParameterOfType(_, messageType))
                    .ToArray();
                if (methods.Length == 0)
                {
                    throw new MissingMethodException($"Type {handlerType} does not provide method for message of type {messageType.Name}");
                }
                foreach (var mthd in methods)
                {
                    var d = mthd.CreateDelegate(handler);
                    Trace.WriteLine($"Subscribing for message {messageType.Name} using method {mthd.Name}");
                    bus.Subscribe(messageType, m => d.DynamicInvoke(m));
                }
            }

            private bool HasOneParameterOfType(MethodInfo methodInfo, Type parameterType)
            {
                return methodInfo
                    .GetParameters()
                    .Where(p => p.ParameterType == parameterType)
                    .Count() == 1;
            }
        }

        class DelegatedHandlerRegistration<TMessage> : HandlerRegistrationBase
        {
            private readonly Action<TMessage> callback;

            public DelegatedHandlerRegistration(Action<TMessage> callback)
            {
                this.callback = callback;
            }

            public override void SubscribeHandler(IMessageBus bus)
            {
                Trace.WriteLine($"Subscribing for message {typeof(TMessage).Name} using delegate");
                bus.Subscribe<TMessage>(callback);
            }
        }
    }
}
