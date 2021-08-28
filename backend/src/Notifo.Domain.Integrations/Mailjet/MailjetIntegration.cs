﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Notifo.Domain.Channels;
using Notifo.Domain.Channels.Email;
using Notifo.Domain.Integrations.Resources;

namespace Notifo.Domain.Integrations.Mailjet
{
    public sealed class MailjetIntegration : IIntegration
    {
        private readonly MailjetEmailServerPool serverPool;

        private static readonly IntegrationProperty ApiKeyProperty = new IntegrationProperty("apiKey", IntegrationPropertyType.Password)
        {
            EditorLabel = Texts.Mailjet_ApiKeyLabel,
            EditorDescription = null,
            IsRequired = true
        };

        private static readonly IntegrationProperty ApiSecretProperty = new IntegrationProperty("apiSecret", IntegrationPropertyType.Password)
        {
            EditorLabel = Texts.Mailjet_ApiSecretLabel,
            EditorDescription = null,
            IsRequired = true
        };

        private static readonly IntegrationProperty FromEmailProperty = new IntegrationProperty("fromEmail", IntegrationPropertyType.Text)
        {
            EditorLabel = Texts.Email_FromEmailLabel,
            EditorDescription = Texts.Email_FromEmailDescription,
            IsRequired = true,
            Summary = true
        };

        private static readonly IntegrationProperty FromNameProperty = new IntegrationProperty("fromName", IntegrationPropertyType.Text)
        {
            EditorLabel = Texts.Email_FromNameLabel,
            EditorDescription = Texts.Email_FromNameDescription,
            IsRequired = true
        };

        public IntegrationDefinition Definition { get; }
            = new IntegrationDefinition(
                "Mailjet",
                Texts.Mailjet_Name,
                "./integrations/mailjet.svg",
                new List<IntegrationProperty>
                {
                    ApiKeyProperty,
                    ApiSecretProperty,
                    FromEmailProperty,
                    FromNameProperty
                },
                new HashSet<string>
                {
                    Providers.Email
                })
            {
                Description = Texts.Mailjet_Description
            };

        public MailjetIntegration(IMemoryCache memoryCache)
        {
            serverPool = new MailjetEmailServerPool(memoryCache);
        }

        public bool CanCreate(Type serviceType, ConfiguredIntegration configured)
        {
            return serviceType == typeof(IEmailSender);
        }

        public object? Create(Type serviceType, ConfiguredIntegration configured)
        {
            if (CanCreate(serviceType, configured))
            {
                var publicKey = ApiKeyProperty.GetString(configured);

                if (string.IsNullOrWhiteSpace(publicKey))
                {
                    return null;
                }

                var privateKey = ApiSecretProperty.GetString(configured);

                if (string.IsNullOrWhiteSpace(privateKey))
                {
                    return null;
                }

                var fromEmail = FromEmailProperty.GetString(configured);

                if (string.IsNullOrWhiteSpace(fromEmail))
                {
                    return null;
                }

                var fromName = FromNameProperty.GetString(configured);

                if (string.IsNullOrWhiteSpace(fromName))
                {
                    return null;
                }

                return new MailjetEmailSender(() => serverPool.GetServer(publicKey, privateKey), fromEmail, fromName);
            }

            return null;
        }
    }
}
