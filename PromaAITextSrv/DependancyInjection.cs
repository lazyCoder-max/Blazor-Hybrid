using PromaAITextSrv.Interfaces;
using PromaAITextSrv.Models;
using PromaAITextSrv.Services;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromaAITextSrv
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddPromanAITextService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRefit(configuration);
            services.AddMessageBroker(configuration);
            return services;
        }
        private static void AddRefit(this IServiceCollection services, IConfiguration configuration)
        {
            var options  = new PromaAITextOptions();
            configuration.GetSection(PromaAITextOptions.SectionName).Bind(options);

            services.AddSingleton(options);

            if(options.X_API_Token == null)
            {
                throw new Exception("X_API_Token is not set in configuration.");
            }
            services.AddRefitClient<IAPIService>().
            ConfigureHttpClient(c => c.BaseAddress = new Uri(options.base_url))
            .AddHttpMessageHandler(() => new AuthHeaderHandler(() => Task.FromResult(options.X_API_Token)));

        }
        private static void AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new MessageBrokerOptions();
            configuration.GetSection(MessageBrokerOptions.Key).Bind(options);

            services.AddSingleton(options);

            services.AddSingleton<IMessageBrokerService, MessageBrokerService>();
        }
        public class AuthHeaderHandler : DelegatingHandler
        {
            private readonly Func<Task<string>> _getToken;

            public AuthHeaderHandler(Func<Task<string>> getToken)
            {
                _getToken = getToken ?? throw new ArgumentNullException(nameof(getToken));
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var token = await _getToken();

                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.TryAddWithoutValidation("X-API-Token", token);
                }

                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}
