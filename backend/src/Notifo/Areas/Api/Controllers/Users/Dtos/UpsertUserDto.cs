﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Notifo.Domain;
using Notifo.Domain.Users;
using Notifo.Infrastructure.Collections;
using Notifo.Infrastructure.Reflection;

namespace Notifo.Areas.Api.Controllers.Users.Dtos;

public sealed class UpsertUserDto
{
    /// <summary>
    /// The id of the user.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// The full name of the user.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// The email of the user.
    /// </summary>
    public string? EmailAddress { get; set; }

    /// <summary>
    /// The phone number.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// The preferred language of the user.
    /// </summary>
    public string? PreferredLanguage { get; set; }

    /// <summary>
    /// The timezone of the user.
    /// </summary>
    public string? PreferredTimezone { get; set; }

    /// <summary>
    /// True when only whitelisted topic are allowed.
    /// </summary>
    public bool? RequiresWhitelistedTopics { get; set; }

    /// <summary>
    /// The user properties.
    /// </summary>
    public ReadonlyDictionary<string, string>? Properties { get; set; }

    /// <summary>
    /// Notification settings per channel.
    /// </summary>
    public Dictionary<string, ChannelSettingDto>? Settings { get; set; }

    public UpsertUser ToUpsert()
    {
        var result = SimpleMapper.Map(this, new UpsertUser());

        if (Settings != null)
        {
            result.Settings = new ChannelSettings();

            foreach (var (key, value) in Settings)
            {
                if (value != null)
                {
                    result.Settings[key] = value.ToDomainObject();
                }
            }
        }

        return result;
    }
}
