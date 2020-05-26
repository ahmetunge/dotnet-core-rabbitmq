using System.Collections.Generic;

namespace Core.Queue
{
    public interface IPublisherService
    {
        void AddQueue<T>(IEnumerable<T> queueDataModels, string queueName) where T : class, new();
    }
}