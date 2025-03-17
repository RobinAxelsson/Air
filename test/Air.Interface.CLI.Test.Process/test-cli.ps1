#!/usr/bin/env pwsh
$ErrorActionPreference = 'Stop'
# if not set the script does not stop on thrown exceptions inside functions

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

function RunTest(){
    InitDevEnv

    Write-Host "Running tests for CLI"

    # Avoid Join-Path to normalize the path (bug when having dots in the path)
    $out_dir = NormalizePath "$PSScriptRoot/out/cli"

    if (Test-Path $out_dir) {
        Remove-Item -Recurse -Force $out_dir
    }

    New-Item -ItemType Directory -Path $out_dir

    dotnet build ${env:_CLI_PROJ_PATH_} -c Release -o $out_dir

    $exe = NormalizePath "$out_dir/Air.Interface.CLI.exe"
    if(!(Test-Path $exe)){
        throw "Could not find path '$exe' after building the CLI"
    }

    # if($args.Length -eq 1){
    #     $coveragePath = $args[0]
    #     dotnet-coverage collect -f cobertura -o $coveragePath -- "$exe sync-fares"
    # }
    # else {

        # Do not change these strings they are used to filter the output of the tests
        Write-Host "-----Start running CLI-----"
        Invoke-Expression "$exe sync-fares"
        Write-Host "--------CLI exited---------"

        # the exception from the exe does not bubble up to the powershell script
        if($LASTEXITCODE -ne 0){
            throw "The dotnet exe file exited with code: $LASTEXITCODE path: $exe"
        }
    # }
}

function InitDevEnv(){
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

RunTest
