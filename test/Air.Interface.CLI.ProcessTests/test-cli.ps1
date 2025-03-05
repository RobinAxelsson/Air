#!/usr/bin/env pwsh
$ErrorActionPreference = 'Stop'
# if not set the script does not stop on thrown exceptions inside functions

function RunTest(){
    InitPwshEnv

    Write-Host "Running tests for CLI"
    $out_dir = Join-Path $PSScriptRoot "out" "cli"

    if (Test-Path $out_dir) {
        Remove-Item -Recurse -Force $out_dir
    }

    New-Item -ItemType Directory -Path $out_dir

    dotnet build ${env:_CLI_PROJ_PATH_} -c Release -o $out_dir

    $exe = Join-Path $out_dir "Air.Interface.CLI.exe"
    if(!(Test-Path $exe)){
        throw "Could not find the dotnet exe in path: $exe"
    }

    if($args.Length -eq 1){
        $coveragePath = $args[0]
        dotnet-coverage collect -f cobertura -o $coveragePath -- $exe
    }
    else {
        Invoke-Expression $exe

        # the exception from the exe does not bubble up to the powershell script
        if($LASTEXITCODE -ne 0){
            throw "The dotnet exe file exited with code: $LASTEXITCODE path: $exe"
        }
    }
    Write-Host "running sql select fares"
    sql-select-fares.ps1
}

function InitPwshEnv(){
    $scriptPath = $PSScriptRoot

    while ($null -ne $scriptPath) {
        $initPath = Join-Path -Path $scriptPath -ChildPath "init.ps1"

        if (Test-Path $initPath) {
            . $initPath

            if(!($env:_pwsh_env_initialized_ -eq $true)){
                throw "init.ps1 did not initialize the environment"
            }

            return
        }

        $scriptPath = Split-Path -Path $scriptPath -Parent
    }

    throw "Unable to initialize the powershell environment for the test, could not find initialization script init.ps1 (should be in root)"
}

RunTest
