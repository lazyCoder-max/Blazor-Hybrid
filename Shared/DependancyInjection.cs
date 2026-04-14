using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Config;
using Shared.Interfaces;
using Shared.Services;

namespace Shared
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddPromanAISharedServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IChatUIService, ChatUIService>();
            var options = new MessageBrokerOptions();
            configuration.GetSection(MessageBrokerOptions.Key).Bind(options);

            services.AddSingleton(options);
            services.AddSingleton<IMessageBrokerService, MessageBrokerService>();
            return services;
        }
    }
}
