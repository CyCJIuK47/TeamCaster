using System;
using System.Collections.Generic;

namespace TeamCaster.Core.Architecture.Observer
{
    class DataPublisher
    {
        private Dictionary<Type, List<ISubscriber>> _subscribers;

        public DataPublisher()
        {
            _subscribers = new Dictionary<Type, List<ISubscriber>>();
        }

        public void AddSubscriber(Type subscribedType, ISubscriber subscriber)
        {
            if (!_subscribers.ContainsKey(subscribedType))
                _subscribers.Add(subscribedType, new List<ISubscriber>());

            _subscribers[subscribedType].Add(subscriber);
        }

        public void RemoveSubscriber(Type subscribedType, ISubscriber subscriber)
        {
            List<ISubscriber> relevantSubscribers = null;
            _subscribers.TryGetValue(subscribedType, out relevantSubscribers);

            relevantSubscribers?.Remove(subscriber);
        }

        public void Publish(dynamic receivedData)
        {
            List<ISubscriber> relevantSubscribers = null;
            _subscribers.TryGetValue(receivedData.GetType(), out relevantSubscribers);

            relevantSubscribers?.ForEach(subscriber => subscriber.Update(receivedData));
        }
    }
}
