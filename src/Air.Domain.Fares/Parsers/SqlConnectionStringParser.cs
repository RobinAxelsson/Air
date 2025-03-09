// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;

namespace Air.Domain;

internal static class SqlConnectionStringParser
{
    public static SqlConnectionString Parse(string connectionString)
    {
        var properties = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries);

        var error = new StringBuilder();

        string? GetValue(string key)
        {
            foreach (var prop in properties)
            {
                var kvp = prop.Split('=', 2);
                if (kvp.Length == 2 && kvp[0].Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    return kvp[1].Trim();
                }
            }

            return null;
        }

        var serverAndPort = GetValue("Server");
        var database = GetValue("Database");
        var userId = GetValue("User Id");
        var password = GetValue("Password");
        var trustServerCertificateValue = GetValue("TrustServerCertificate");


        (int? port, string? server) GetServerAndPort(string? serverAndPort)
        {
            var parts = serverAndPort?.Split(',');

            if (parts is null || parts.Length == 0)
                return (null, null);

            if (parts.Length == 1)
                return (null, parts[0]);

            _ = int.TryParse(parts[1], out int port);
            return (port, parts[0]);
        }

        var (port, server) = GetServerAndPort(serverAndPort);

        if (string.IsNullOrWhiteSpace(server))
            error.AppendLine("- Server required eg 'localhost'.");

        if (port is null)
            error.AppendLine("- Port required eg '1433'. ");

        if (port != null && (port <= 0 || port >= 65535))
            error.AppendLine("- Port must be between 1 and 65535. ");

        if (string.IsNullOrWhiteSpace(database))
            error.AppendLine("- Database is required. ");

        if (string.IsNullOrWhiteSpace(userId))
            error.AppendLine("- UserId is required. ");

        if (string.IsNullOrWhiteSpace(password))
            error.AppendLine("- Password is required.");

        if (trustServerCertificateValue is null)
            error.AppendLine("- TrustServerCertificate is required. ");

        bool trustServerCertificate = false;
        if (trustServerCertificateValue != null && !bool.TryParse(GetValue("TrustServerCertificate"), out trustServerCertificate))
        {
            error.AppendLine("- TrustServerCertificate must be a boolean value. ");
        }

        if (error.Length > 0)
        {
            error.Insert(0, "Example: Server={Server},{Port};Database={Database};User Id={UserId};Password={Password};TrustServerCertificate={TrustServerCertificate};\n");
            throw new InvalidConnectionStringException(error.ToString());
        }

        return new SqlConnectionString
        {
            Host = server!,
            Port = port!.Value,
            Database = database!,
            UserId = userId!,
            Password = password!,
            TrustServerCertificate = trustServerCertificate
        };
    }
}
