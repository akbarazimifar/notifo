﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Notifo.Domain.Channels;
using Notifo.Domain.Channels.Webhook;
using Notifo.Domain.Channels.Webhook.Integrations;
using Notifo.Domain.Integrations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebhookServiceExtensions
    {
        public static void AddMyWebhookChannel(this IServiceCollection services)
        {
            services.AddSingletonAs<WebhookChannel>()
                .As<ICommunicationChannel>();

            services.AddSingletonAs<WebhookIntegration>()
                .As<IIntegration>();

            services.AddScheduler<WebhookJob>(Providers.Webhook);
        }
    }
}
