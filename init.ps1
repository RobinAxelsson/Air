#!/usr/bin/env pwsh
# dot source this script to initialize the environment > . ./init.ps1

# if 'Stop' not set the script does not stop on thrown exceptions inside functions
$ErrorActionPreference = 'Stop'

# wrap the functions to avoid polluting the global
function InitRepositoryEnvironment(){
    function Main(){
        $scriptsPath = Join-Path $PSScriptRoot scripts
        $airInterfaceCliProcessTests = Join-Path $PSScriptRoot test Air.Interface.CLI.ProcessTests
        $airDomainFaresPath = Join-Path $PSScriptRoot src Air.Domain.Fares
        $airInterfaceCliPath = Join-Path $PSScriptRoot src Air.Interface.CLI
        $testResultsPath = Join-Path $PSScriptRoot temp test-results

        EnsurePathsExist $scriptsPath, $airDomainFaresPath, $airInterfaceCliPath, $airInterfaceCliProcessTests, $testResultsPath

        # add script paths to run dev scripts
        AddDirToEnvPath $scriptsPath
        AddDirToEnvPath $airInterfaceCliProcessTests

        # add environment variables (some scripts depend on these)
        $env:_PWSH_ENV_INITIALIZED_ = $true
        $env:_REPO_NAME_ = "Air"
        $env:_REPO_ROOT_ = $PSScriptRoot
        $env:_EF_PROJ_PATH_ = $airDomainFaresPath
        $env:_CLI_PROJ_PATH_ = $airInterfaceCliPath
        $env:_TEST_RESULTS_PATH_ = $testResultsPath
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
#nest these functions to avoid polluting the global scope
InitRepositoryEnvironment
