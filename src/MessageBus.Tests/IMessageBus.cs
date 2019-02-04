using System;

namespace DDD
{
    internal interface IMessageBus
    {
        void Subscribe(Type messageType, Action<object> handler);
        void Subscribe<T>(Action<T> handler);
        void Publish(object message);
    }
}