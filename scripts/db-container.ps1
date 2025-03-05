#!/usr/bin/env pwsh
param(
    [string]$action
)
$ErrorActionPreference = 'Stop'

function Main(){
    function HelpText(){
        Write-Host "Bad arguments, usage: db-container <create|remove|start|stop>"
    }

    if(-not $action -or $args.Length -gt 1){
       HelpText
       Exit 1
    }

    if($action -ne "create" -and $action -ne "remove" -and $action -ne "start" -and $action -ne "stop"){
        HelpText
        Exit 1
    }

    if($action -eq "start" -or $action -eq "stop"){
        StartStopDbContainer
        Exit 0
    }

    if($action -eq "create" -or $action -eq "remove"){
        CreateRemoveDbContainer
        Exit 0
    }
}

function StartStopDbContainer(){
    $containerName = "air_sql_server"
    $container = docker ps -a --filter "name=$containerName" --format "{{.ID}}"

    if(!($container)){
      Write-Host "Container '$containerName' not found: docker ps -a"
      Exit 1
    }

    if($action -eq "start"){
        Write-Host "Starting container '${containerName}'..."
        docker start $container
    }

    if($action -eq "stop"){
        Write-Host "Stopping container '${containerName}'..."
        docker stop $container
    }
}

function CreateRemoveDbContainer(){
    $dockerCompose = Join-Path $PSScriptRoot subs docker-compose.db.yml

    if (-not (Test-Path $dockerCompose)) {
        throw "docker-compose not found in path: $dockerCompose"
    }

    if($action -eq "remove"){
        docker-compose -f $dockerCompose down
        return
    }

    if($action -eq "create"){
        docker-compose -f $dockerCompose up -d

        Start-Sleep 2
        Write-Host "$ Updating with database with dotnet-ef..."
        dotnet-ef database update --startup-project $env:_EF_PROJ_PATH_
        return
    }

    throw "Not implemented"
}

Main
