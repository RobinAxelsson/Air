// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

//We use the same file for easier maintenance and easier comparison
namespace Air.Domain;


[ExcludeFromCodeCoverage]
public sealed class InvalidConnectionStringException : AirFaresTechnicalBaseException
{
    public override string Reason => "Invalid connedction string";

    public InvalidConnectionStringException(string message)
        : base(message)
    {
    }
}


[ExcludeFromCodeCoverage]
public sealed class RyanairServiceRequestException : AirFaresTechnicalBaseException
{
    public override string Reason => "Configuration Setting Missing";

    public RyanairServiceRequestException(string message, object properties) : base(message += Environment.NewLine + properties.JsonSerializerSerializePretty())
    {
    }
}

[ExcludeFromCodeCoverage]
public sealed class DbContextReturnNullException : AirFaresTechnicalBaseException
{
    public override string Reason => "Data access read failed";

    public DbContextReturnNullException(string message, object properties) : base(message += Environment.NewLine + properties.JsonSerializerSerializePretty())
    {
    }
}

[ExcludeFromCodeCoverage]
public sealed class ConfigurationSettingException : AirFaresTechnicalBaseException
{
    public override string Reason => "Configuration Setting Missing";

    public ConfigurationSettingException(string message)
        : base(message)
    {
    }
}

[ExcludeFromCodeCoverage]
public sealed class AirSqlConnectionException : AirFaresTechnicalBaseException
{
    public override string Reason => "Unable to connect to sql server";

    public AirSqlConnectionException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

[ExcludeFromCodeCoverage]
public sealed class DbContextCreateAirFlightsException : AirFaresTechnicalBaseException
{
    public override string Reason => "";
    public DbContextCreateAirFlightsException(string message, object properties, Exception inner) : base(message += Environment.NewLine + properties.JsonSerializerSerializePretty(), inner)
    {
    }
}

[ExcludeFromCodeCoverage]
public sealed class DbContextUpdateAirFlightsException : AirFaresTechnicalBaseException
{
    public override string Reason => "";
    public DbContextUpdateAirFlightsException(string message, object properties, Exception inner) : base(message += Environment.NewLine + properties.JsonSerializerSerializePretty(), inner)
    {
    }
}

[ExcludeFromCodeCoverage]
public sealed class DbContextGetAirFlightsException : AirFaresTechnicalBaseException
{
    public override string Reason => "";
    public DbContextGetAirFlightsException(string message, object properties, Exception inner) : base(message += Environment.NewLine + properties.JsonSerializerSerializePretty(), inner)
    {
    }
}
