using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.DependencyResolvers
{
    public class LoggingModule : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddLogging(conf => conf.AddConsole());
        }
    }
}