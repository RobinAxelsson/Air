#!/usr/bin/env pwsh
$ErrorActionPreference = 'Stop'

function Main(){
    EnsureDevDependenciesInstalled
}

function EnsureDevDependenciesInstalled() {
    # Define dependencies with installation instructions
    $devDependencies = @{
        'dotnet'           = 'Install .NET SDK 9: https://dotnet.microsoft.com/en-us/download'
        'pwsh'             = 'Install PowerShell: https://github.com/PowerShell/PowerShell'
        'git'              = 'Install Git: https://git-scm.com/downloads'
        'python'           = 'Install Python: https://www.python.org/downloads/'
        'docker'           = 'Install Docker: https://www.docker.com/get-started'
        'junit2html'       = 'pip install junit2html'
        'air-watch'        = 'airwatch-install -else .\tools\airwatch-install.ps1'
        'dotnet-ef'        = 'dotnet tool install --global dotnet-ef'
        'dotnet-coverage'  = 'dotnet tool install --global dotnet-coverage'
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
