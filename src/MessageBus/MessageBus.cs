using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD
{
    public class MessageBus : IMessageBus
    {
        private readonly List<Route> routes = new List<Route>();

        public void Publish(object message)
        {
            var messageType = message.GetType();
            var routesForThisMessage = GetRoutes(messageType);
            var deadRefs = routesForThisMessage
                .Where(_ => _.Target == null)
                .ToArray();
            foreach (var r in deadRefs)
            {
                routes.Remove(r);
            }
            var liveRefs = routesForThisMessage.Except(deadRefs);
            foreach (var r in liveRefs)
            {
                InvokeHandler(r.Target, message);
            }
        }

        public void InvokeHandler(Action<object> handler, object message)
        {
            try
            {
                handler.Invoke(message);
            }
            catch { }
        }

        private IEnumerable<Route> GetRoutes(Type messageType)
        {
            return routes
                .Where(_ => _.MessageType.IsAssignableFrom(messageType))
                .ToArray();
        }

        public void Subscribe(Type messageType, Action<object> handler)
        {
            routes.Add(new Route(messageType, handler));
        }
    }
}