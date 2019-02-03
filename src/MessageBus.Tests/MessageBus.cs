using System;

namespace DDD
{
    internal class MessageBus : IMessageBus
    {
        public void Subscribe<T>(Action<T> handler)
        {
            throw new NotImplementedException();
        }
    }
}