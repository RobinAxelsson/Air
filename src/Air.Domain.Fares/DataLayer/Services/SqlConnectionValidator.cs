// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net.NetworkInformation;
using System.Net.Sockets;
using Microsoft.Data.SqlClient;

namespace Air.Domain
{
    internal static class SqlConnectionValidator
    {
        internal static void EnsureConnection(SqlConnectionString cs)
        {
            EnsureHostIsUp(cs);

            EnsureDbServerIsUp(cs);

            SqlConnection connection = null!;
            try
            {
                connection = EnsureLoginIsWorking(cs);
                EnsureDatabaseIsReachable(cs, connection);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        private static void EnsureHostIsUp(SqlConnectionString cs)
        {
            try
            {
                using var ping = new Ping();
                ping.Send(cs.Host);
            }
            catch (PingException ex)
            {
                throw new AirSqlConnectionException($"Failed to ping host: '{cs.Host}'", ex);
            }
        }

        private static void EnsureDbServerIsUp(SqlConnectionString cs)
        {
            try
            {
                using var tcpClient = new TcpClient();
                tcpClient.Connect(cs.Host, cs.Port);
            }
            catch (SocketException ex)
            {
                throw new AirSqlConnectionException($"Unable to connect to server. host: '{cs.Host}' port: '{cs.Port}'", ex);
            }
        }

        private static SqlConnection EnsureLoginIsWorking(SqlConnectionString cs)
        {
            SqlConnection connection;
            try
            {
                connection = new SqlConnection(cs.ToConnectionStringWithoutDatabase());
                connection.Open();
            }
            catch (SqlException ex)
            {
                throw new AirSqlConnectionException($"Could not login to sql server host: '{cs.Host}', port: '{cs.Port}', UserId: '{cs.UserId}'", ex);
            }

            return connection;
        }

        private static void EnsureDatabaseIsReachable(SqlConnectionString cs, SqlConnection connection)
        {
            try
            {
                using (var command = new SqlCommand($"USE [{cs.Database}];", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new AirSqlConnectionException($"Could NOT access database from valid login. database: '{cs.Database}' server: '{cs.Host}' userId: '{cs.UserId}' , TrustServerCertificate: '{cs.TrustServerCertificate}'", ex);
            }
        }
    }
}
