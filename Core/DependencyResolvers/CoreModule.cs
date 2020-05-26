using Core.Queue;
using Microsoft.Extensions.DependencyInjection;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddScoped<IQueueService, RabbitMQQueueService>();
        }
    }
}