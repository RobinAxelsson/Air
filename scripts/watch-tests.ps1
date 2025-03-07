#!/usr/bin/env pwsh
$watchPath = "."
$watcher = New-Object System.IO.FileSystemWatcher
$watcher.Path = $watchPath
$watcher.Filter = "*.cs"
$watcher.IncludeSubdirectories = $true
$watcher.NotifyFilter = [System.IO.NotifyFilters]'FileName, LastWrite'

$script:testRunning = $false

$action = {
    Write-Host "File change detected: $($Event.SourceEventArgs.FullPath)."
    if (-not $script:testRunning) {
        $script:testRunning = $true

        try {
            $output = & powershell -Command "dotnet test" | Out-String
            Write-Host $output
        }
        catch {
            Write-Host "Error executing script: $_"
        }


        # for ($i = 0; $i -lt 3; $i++) {
        #     Write-Host "Doing work... $i"
        #     Start-Sleep -Seconds 1
        # }
        $script:testRunning = $false
    }
}

# Register the event. "Changed" covers file modifications. You could also register for "Created" if needed.
$null = Register-ObjectEvent -InputObject $watcher -EventName "Changed" -SourceIdentifier "CsFileChanged" -Action $action

$watcher.EnableRaisingEvents = $true

Write-Host "Watching for .cs file changes in $watchPath"

try {
    while ($true) {
        Start-Sleep -Seconds 1
    }
}
finally {
    Write-Host "Cleaning up..."
    $watcher.EnableRaisingEvents = $false
    Unregister-Event -SourceIdentifier "CsFileChanged"
    $watcher.Dispose()
    Write-Host "Exited successfully."
}
