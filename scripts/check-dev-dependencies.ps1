#!/usr/bin/env pwsh
$ErrorActionPreference = 'Stop'

function Main(){
    EnsureDevDependenciesInstalled
}

function EnsureDevDependenciesInstalled() {
    # Define dependencies with installation instructions
    $devDependencies = @{
        'dotnet'           = 'Install .NET SDK 9: https://dotnet.microsoft.com/en-us/download'
        'pwsh'             = 'Install PowerShell Core: https://github.com/PowerShell/PowerShell, sudo apt install powershell, winget install -e --id Microsoft.PowerShell --accept-package-agreements'
        'git'              = 'Install Git: https://git-scm.com/downloads winget install -e --id Git.Git --accept-package-agreements'
        'docker'           = 'Install Docker: https://www.docker.com/get-started'
        'sqlcmd'           = 'Install SQL Server Command Line Tools: https://docs.microsoft.com/en-us/sql/tools/sqlcmd-utility, sudo apt install sqlcmd'
        'dotnet-ef'        = 'dotnet tool install --global dotnet-ef'
        'dotnet-coverage'  = 'dotnet tool install --global dotnet-coverage'
        'gh'               = 'sudo apt install gh, winget install -e GitHub.cli --accept-package-agreements'
    }

    $missingDependencies = @{}

    foreach ($dependency in $devDependencies.Keys) {
        try {
            $null = Get-Command $dependency -ErrorAction Stop
        }
        catch {
            $missingDependencies[$dependency] = $devDependencies[$dependency]
        }
    }

    if ($missingDependencies.Count -gt 0) {
        Write-Output "`nThe following dependencies are missing:"
        foreach ($key in $missingDependencies.Keys) {
            Write-Output "❌ $key → Install using: $($missingDependencies[$key])"
        }
        Exit 1
    }
    else {
        Write-Output "✅ All dev dependencies are installed!"
    }
}

Main
