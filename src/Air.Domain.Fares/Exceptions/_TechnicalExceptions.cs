// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

//We use the same file for easier maintenance and easier comparison
namespace Air.Domain;


[ExcludeFromCodeCoverage]
public sealed class InvalidConnectionStringException : AirFaresTechnicalBaseException
{
    public override string Reason => "Invalid connedction string";

    public InvalidConnectionStringException()
    {
    }

    public InvalidConnectionStringException(string message)
        : base(message)
    {
    }

    public InvalidConnectionStringException(string message, Exception inner)
        : base(message, inner)
    {
    }
}


[ExcludeFromCodeCoverage]
public sealed class RyanairServiceRequestException : AirFaresTechnicalBaseException
{
    public override string Reason => "Configuration Setting Missing";

    public RyanairServiceRequestException() : base()
    {
    }

    public RyanairServiceRequestException(string message, object properties) : base(message += Environment.NewLine + properties.JsonSerializerSerializeWriteIndented())
    {
    }
}

[ExcludeFromCodeCoverage]
public sealed class DbContextReturnNullException : AirFaresTechnicalBaseException
{
    public override string Reason => "Data access read failed";

    public DbContextReturnNullException()
    {
    }

    public DbContextReturnNullException(string message)
        : base(message)
    {
    }

    public DbContextReturnNullException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

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

[ExcludeFromCodeCoverage]
public sealed class AirSqlConnectionException : AirFaresTechnicalBaseException
{
    public override string Reason => "Unable to connect to sql server";

    public AirSqlConnectionException()
    {
    }

    public AirSqlConnectionException(string message)
        : base(message)
    {
    }

    public AirSqlConnectionException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
