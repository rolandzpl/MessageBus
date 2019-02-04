using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD
{
    public class MessageBus : IMessageBus
    {
        private readonly Dictionary<Type, List<Action<Object>>> routes =
            new Dictionary<Type, List<Action<object>>>();

        public void Publish(object message)
        {
            var messageType = message.GetType();
            var handlers = GetHandlers(messageType);
            if (!handlers.Any())
            {
                return;
            }
            foreach (var h in handlers)
            {
                TryPublishInternal(h, message);
            }
        }

        private IEnumerable<Action<object>> GetHandlers(Type messageType)
        {
            return !routes.TryGetValue(messageType, out List<Action<object>> result) ?
                new List<Action<object>>() :
                result;
        }

        private static void TryPublishInternal(Action<object> handler, object message)
        {
            try
            {
                handler.Invoke(message);
            }
            catch { }
        }

        public void Subscribe(Type messageType, Action<object> handler)
        {
            List<Action<object>> handlers;
            if (!routes.TryGetValue(messageType, out handlers))
            {
                handlers = new List<Action<object>>();
                routes.Add(messageType, handlers);
            }
            handlers.Add(handler);
        }

        public void Subscribe<T>(Action<T> handler)
        {
            Subscribe(typeof(T), (object o) => handler.Invoke((T)o));
        }
    }
}