using System;

namespace DDD
{
    internal interface IMessageBus
    {
        void Subscribe<T>(Action<T> handler);
        void Publish(object message);
    }
}