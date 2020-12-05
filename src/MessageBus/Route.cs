using System;

namespace DDD
{
    class Route
    {
        public Route(Type messageType, Action<object> action)
        {
            MessageType = messageType;
            HandlerRef = new WeakReference<Action<object>>(action);
        }

        public WeakReference<Action<object>> HandlerRef { get; }

        public Type MessageType { get; }

        public Action<object> Target
        {
            get { return GetTarget(); }
        }

        private Action<object> GetTarget()
        {
            Action<object> target;
            HandlerRef.TryGetTarget(out target);
            return target;
        }

        public void InvokeHandler(object message)
        {
            try
            {
                Target.Invoke(message);
            }
            catch { }
        }
    }
}