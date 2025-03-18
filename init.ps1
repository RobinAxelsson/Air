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
        Write-Host "root: $root"
        $scriptsPath = Join-Path $root "scripts"

        # Avoid Join-Path to normalize the path (bug when having dots in the path)
        $airInterfaceCliProcessTests = NormalizePath "$root/test/Air.Interface.CLI.Test.Process"
        $airDomainFaresPath = NormalizePath "$root/src/Air.Domain.Fares"
        $airInterfaceCliPath = NormalizePath "$root/src/Air.Interface.CLI"
        $toolsPath = NormalizePath "$root/tools"

        EnsurePathsExist @(
            $scriptsPath,
            $airDomainFaresPath,
            $airInterfaceCliPath,
            $airInterfaceCliProcessTests,
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
        if (-not ($env:PATH -split [System.IO.Path]::PathSeparator -contains $dir)) {
            $env:PATH += [System.IO.Path]::PathSeparator + $dir
        }
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
