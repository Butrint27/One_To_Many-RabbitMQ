﻿using Messaging.Shared.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMessagingServices(this IServiceCollection services)
        {
            services.AddMassTransitConfiguration();
        }
    }
}
