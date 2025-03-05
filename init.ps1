#!/usr/bin/env pwsh
# dot source this script to initialize the environment > . ./init.ps1

# if 'Stop' not set the script does not stop on thrown exceptions inside functions
$ErrorActionPreference = 'Stop'

# wrap the functions to avoid polluting the global scope
function InitDevEnvironment()
{
    function Main(){
        SetupEnvironementVariables
        Invoke-Expression check-dev-dependencies.ps1
    }

    function SetupEnvironementVariables(){
        $root = $PSScriptRoot
        $scriptsPath = Join-Path $root scripts
        $airInterfaceCliProcessTests = Join-Path $root test Air.Interface.CLI.ProcessTests
        $airDomainFaresPath = Join-Path $root src Air.Domain.Fares
        $airInterfaceCliPath = Join-Path $root src Air.Interface.CLI
        $toolsPath = Join-Path $root tools
        $testResultsPath = Join-Path $root temp test-results

        EnsurePathsExist @(
            $scriptsPath,
            $airDomainFaresPath,
            $airInterfaceCliPath,
            $airInterfaceCliProcessTests,
            $testResultsPath,
            $toolsPath)

        # add script paths to run dev scripts
        AddDirToEnvPath $scriptsPath
        AddDirToEnvPath $airInterfaceCliProcessTests
        AddDirToEnvPath $toolsPath

        # add environment variables (some scripts depend on these)
        $env:_DEV_ENV_INITIALIZED_ = $true
        $env:_REPO_NAME_ = "Air"
        $env:_REPO_ROOT_ = $root
        $env:_EF_PROJ_PATH_ = $airDomainFaresPath
        $env:_CLI_PROJ_PATH_ = $airInterfaceCliPath
        $env:_TEST_RESULTS_PATH_ = $testResultsPath
        $env:_TOOLS_PATH_ = $toolsPath

        Write-Host "✅ Dev Path updated and environment variables initialized"
    }

    function AddDirToEnvPath($dir){
        if (-not ($env:Path -split [System.IO.Path]::PathSeparator -contains $dir)) {
            $env:Path += [System.IO.Path]::PathSeparator + $dir
        }
    }

    function EnsurePathsExist {
        [CmdletBinding()]
        param (
            [Parameter(Mandatory = $true)]
            [string[]]$paths
        )

        $invalidPaths = @()

        foreach ($item in $paths) {
            if (-not (Test-Path $item)) {
                $invalidPaths += $item
            }
        }

        if($invalidPaths.Count -gt 0){
            throw "Some paths that the powershell script environment needs are missing : $invalidPaths"
        }
    }

    Main
}

InitDevEnvironment
