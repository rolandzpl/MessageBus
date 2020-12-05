using System;

namespace DDD
{
    public interface IMessageBus
    {
        void Subscribe(Type messageType, Action<object> handler);
        
        void Publish(object message);
    }
}