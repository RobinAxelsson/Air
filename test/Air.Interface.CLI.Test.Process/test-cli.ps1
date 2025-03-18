#!/usr/bin/env pwsh
$ErrorActionPreference = 'Stop'
# if not set the script does not stop on thrown exceptions inside functions

function RunTest(){
    Write-Host "Running tests for CLI"
    # Avoid Join-Path to normalize the path (bug when having dots in the path)
    $out_dir = NormalizePath "$PSScriptRoot/out/cli"

    InitEnvironmentPaths
    $cli_proj = $env:_CLI_PROJ_PATH_
    Write-Host "Building CLI project: $cli_proj"
    dotnet build $cli_proj -c Release -o $out_dir

    if($env:OS -eq "Windows_NT"){
        $exe = NormalizePath "$out_dir/Air.Interface.CLI.exe"
    }
    else{
        $exe = "$out_dir/Air.Interface.CLI"
    }

    if(!(Test-Path $exe)){
        throw "Could not find path '$exe' after building the CLI"
    }

    Write-Host "-----Start running CLI-----"
    Invoke-Expression "$exe sync-fares"
    Write-Host "--------CLI exited---------"

    if($LASTEXITCODE -ne 0){
        throw "The dotnet exe file exited with code: $LASTEXITCODE path: $exe"
    }
}

function InitEnvironmentPaths {
    $scriptPath = $PSScriptRoot

    while ($null -ne $scriptPath) {
        $initPath = NormalizePath "$scriptPath/init.ps1"

        if (Test-Path $initPath) {
            . $initPath

            if(!($env:_DEV_ENV_INITIALIZED_)){
                throw "init.ps1 did not initialize the environment"
            }

            return
        }

        $scriptPath = Split-Path -Path $scriptPath -Parent
    }

    throw "Unable to initialize the powershell environment for the test, could not find initialization script init.ps1 (should be in root)"
}

function NormalizePath {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true)]
        [string] $path
    )

    $separator = [IO.Path]::DirectorySeparatorChar
    $invalidSeparator = [IO.Path]::AltDirectorySeparatorChar
    $path = $path -replace $invalidSeparator, $separator

    return $path
}

RunTest
