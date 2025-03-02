#!/usr/bin/env pwsh

function wait-for-tcp($port){
    for ($i = 0; $i -lt 3; $i++) {
        $result = Test-Connection localhost -TCPPort $port -Count 1
        if($result) {
            break;
        }
    }
}

$database = Join-Path $PSScriptRoot docker-compose.db.yml
docker-compose -f $database up -d

if($args -contains "--update-db"){
    Write-Host "$ Updating database..."
    wait-for-tcp 1433
    Sleep 2
    ef-update
}

docker ps
