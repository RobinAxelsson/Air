#!/usr/bin/env pwsh

#nest these functions to avoid polluting the global scope
function init_repository_environment(){
    function main(){
        $scriptsPath = Join-Path $PSScriptRoot scripts
        $endToEndIntegrationTests = Join-Path $PSScriptRoot test EndToEndIntegrationTests
        $airDomainFaresPath = Join-Path $PSScriptRoot src Air.Domain.Fares
        $airInterfaceCliPath = Join-Path $PSScriptRoot src Air.Interface.CLI
        $testResultsPath = Join-Path $PSScriptRoot temp test-results

        EnsurePathExists $scriptsPath
        EnsurePathExists $airDomainFaresPath
        EnsurePathExists $airInterfaceCliPath
        EnsurePathExists $endToEndIntegrationTests
        EnsurePathExists $testResultsPath

        # add script paths to run dev scripts
        AddDirToEnvPath $scriptsPath
        AddDirToEnvPath $endToEndIntegrationTests

        # add environment variables (some scripts depend on these)
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

    function EnsurePathExists($path){
        if (-not (Test-Path $path)) {
            throw "Path does not exist: $path"
        }
    }

    main
}

init_repository_environment
