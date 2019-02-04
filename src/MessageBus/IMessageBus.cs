using System;

namespace DDD
{
    public interface IMessageBus
    {
        void Subscribe(Type messageType, Action<object> handler);
        void Subscribe<T>(Action<T> handler);
        void Publish(object message);
    }
}