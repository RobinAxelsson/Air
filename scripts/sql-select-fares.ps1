#!/usr/bin/env pwsh
$server = "localhost"
$database = "Air.db"
$username = "sa"
$password = "YourStrong@Passw0rd"
$query = "SELECT * FROM dbo.FlightFares;"

$connectionString = "Server=$server;Database=$database;User Id=$username;Password=$password;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)

try {
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = $query
    $adapter = New-Object System.Data.SqlClient.SqlDataAdapter $cmd
    $dataset = New-Object System.Data.DataSet
    $adapter.Fill($dataset)

    $dataset.Tables[0] | Format-Table -AutoSize
}
catch {
    Write-Host "Error: $_" -ForegroundColor Red
}
finally {
    $conn.Close()
}
