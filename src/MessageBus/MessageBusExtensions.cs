using System;

namespace DDD
{
    public static class MessageBusExtensions
    {
        public static void Subscribe<T>(this IMessageBus _this, Action<T> handler)
        {
            _this.Subscribe(typeof(T), (object o) => handler.Invoke((T)o));
        }
    }
}