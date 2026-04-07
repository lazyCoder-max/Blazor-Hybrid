using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Shared.Interfaces;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddPromanAISharedServices(this IServiceCollection services)
        {
            services.AddSingleton<IChatUIService, ChatUIService>();
            return services;
        }
    }
}
