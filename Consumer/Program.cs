using System;
using System.Text;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Queue;
using Microsoft.Extensions.DependencyInjection;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = StartServices().BuildServiceProvider();

            var queueService = serviceProvider.GetService<IQueueService>();

            queueService.ReadFromQueue();

            Console.ReadLine();
        }
        public static IServiceCollection StartServices()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDependencyResolvers(new ICoreModule[]{
                new CoreModule(),
                new LoggingModule()
            });

            return services;
        }
    }
}
