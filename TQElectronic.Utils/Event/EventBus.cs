using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace TQElectronic.Utils.Event
{
    public sealed class EventBus
    {
        private readonly ConcurrentDictionary<object, ConcurrentBag<Subscriber>> _containerList;
        private IThreadHelper _threadHelper;

        public static EventBus Default { get; } = new EventBus();

        public void SetThreadHelper(IThreadHelper threadHelper)
        {
            this._threadHelper = threadHelper;
            lock (this._containerList)
            {
                foreach (KeyValuePair<object, ConcurrentBag<Subscriber>> container in this._containerList)
                {
                    foreach (Subscriber subscriber in container.Value)
                        subscriber.SetThreadHelper(threadHelper);
                }
            }
        }

        public void Register(object container)
        {
            lock (this._containerList)
            {
                if (this._containerList.ContainsKey(container))
                    return;
                ConcurrentBag<Subscriber> concurrentBag = this.CollectSubscribers(container);
                this._containerList.TryAdd(container, concurrentBag);
            }
        }

        public void Unregister(object container)
        {
            lock (this._containerList)
            {
                ConcurrentBag<Subscriber> concurrentBag;
                this._containerList.TryRemove(container, out concurrentBag);
            }
        }

        public void Post(object message)
        {
            if (message == null)
                return;
            lock (this._containerList)
            {
                foreach (KeyValuePair<object, ConcurrentBag<Subscriber>> container in this._containerList)
                {
                    object key = container.Key;
                    foreach (Subscriber subscriber in container.Value)
                        subscriber.Execute(key, message);
                }
            }
        }

        private ConcurrentBag<Subscriber> CollectSubscribers(object container)
        {
            IEnumerable<MethodInfo> runtimeMethods = container.GetType().GetRuntimeMethods();
            ConcurrentBag<Subscriber> concurrentBag = new ConcurrentBag<Subscriber>();
            foreach (MethodInfo methodInfo in runtimeMethods)
            {
                SubscribeAttribute customAttribute = methodInfo.GetCustomAttribute<SubscribeAttribute>();
                if (customAttribute != null)
                {
                    Subscriber subscriber = new Subscriber(customAttribute, methodInfo);
                    subscriber.SetThreadHelper(this.GetThreadHelper());
                    concurrentBag.Add(subscriber);
                }
            }
            return concurrentBag;
        }

        private IThreadHelper GetThreadHelper()
        {
            return this._threadHelper;
        }

        public EventBus()
        {
            this._containerList = new ConcurrentDictionary<object, ConcurrentBag<Subscriber>>();
            this._threadHelper = (IThreadHelper)new NonUiThreadHelper();
        }
    }
}