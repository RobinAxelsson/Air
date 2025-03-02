#!/usr/bin/env pwsh

# Arrange
$out_dir = Join-Path $PSScriptRoot "out" "cli"

Remove-Item -Recurse -Force $out_dir

if (-not (Test-Path $out_dir)) {
    New-Item -ItemType Directory -Path $out_dir
}


dotnet build ${env:_CLI_PROJ_PATH_} -c Release -o $out_dir
docker-up

$exe = Join-Path $out_dir "Air.Interface.CLI.exe"

# Act
Invoke-Expression $exe

# Assert
sql-select-fares
