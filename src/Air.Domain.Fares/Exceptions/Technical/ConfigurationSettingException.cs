// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Air.Domain;

[ExcludeFromCodeCoverage]
public sealed class ConfigurationSettingException : AirFaresTechnicalBaseException
{
    public override string Reason => "Configuration Setting Missing";

    public ConfigurationSettingException()
    {
    }

    public ConfigurationSettingException(string message)
        : base(message)
    {
    }

    public ConfigurationSettingException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
