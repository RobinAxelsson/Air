#!/usr/bin/env pwsh

$path = "C:\Users\axels\rax\Air\src\Air.Domain.Fares\Services\Ryanair\Dtos"
$filter = "*.cs"
$command = "test-run"

if (!(Test-Path $path)) {
    throw "Path not found: $path"
}

# Create FileSystemWatcher
$watcher = New-Object System.IO.FileSystemWatcher
$watcher.Path = $path
$watcher.Filter = $filter
$watcher.NotifyFilter = [System.IO.NotifyFilters]::LastWrite
$watcher.IncludeSubdirectories = $true  # Watches subdirectories too

# Dictionary to track last event timestamps
$eventTimestamps = @{}

# Action to perform when a file is changed
$action = {
    param($source, $event)

    $file = $event.FullPath
    $now = Get-Date

    $eventTimestamps[$file] = $now

    Write-Host "File changed: $file"

    # Run command
    & $command
}

# Register event handler
$registered = Register-ObjectEvent -InputObject $watcher -EventName Changed -Action $action

# Start watching
$watcher.EnableRaisingEvents = $true
Write-Host "Watching for changes in $path with filter '$filter'. Press Ctrl+C to stop."

# Keep the script running
try {
    while ($true) { Start-Sleep -Seconds 2 }
} finally {
    # Cleanup on exit
    Unregister-Event -SourceIdentifier $registered.Name
    $watcher.Dispose()
    Write-Host "Watcher stopped."
}
