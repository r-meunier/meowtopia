param([string]$Tool, [string]$ArgumentsJson = '{}', [string]$Resource)
$ErrorActionPreference = 'Stop'
$uri = 'http://127.0.0.1:8080/mcp'
$headers = @{ Accept = 'application/json, text/event-stream' }
function Send-Mcp($payload) {
    Invoke-WebRequest -Uri $uri -Method Post -ContentType 'application/json' -Headers $headers -Body ($payload | ConvertTo-Json -Depth 30 -Compress) -TimeoutSec 30
}
function Show-Response($response) {
    foreach ($line in ($response.Content -split "`n")) {
        if ($line.StartsWith('data: ')) {
            $message = $line.Substring(6) | ConvertFrom-Json
            if ($message.result.structuredContent) { $message.result.structuredContent | ConvertTo-Json -Depth 30 }
            elseif ($message.result.contents) { $message.result.contents | ForEach-Object { $_.text } }
            elseif ($message.result.content) { $message.result.content | ForEach-Object { $_.text } }
            elseif ($message.error) { $message.error | ConvertTo-Json }
        }
    }
}
$init = Send-Mcp @{ jsonrpc='2.0'; id=1; method='initialize'; params=@{protocolVersion='2024-11-05';capabilities=@{};clientInfo=@{name='Meowtopia diagnostics';version='1.0'}} }
$headers['Mcp-Session-Id'] = [string]$init.Headers['Mcp-Session-Id']
$null = Send-Mcp @{jsonrpc='2.0';method='notifications/initialized'}
if ($Resource) {
    Show-Response (Send-Mcp @{jsonrpc='2.0';id=2;method='resources/read';params=@{uri=$Resource}})
} elseif ($Tool) {
    Show-Response (Send-Mcp @{jsonrpc='2.0';id=2;method='tools/call';params=@{name=$Tool;arguments=($ArgumentsJson | ConvertFrom-Json -AsHashtable)}})
} else {
    foreach ($resource in @('mcpforunity://project/info','mcpforunity://editor/state','mcpforunity://custom-tools')) {
        (Send-Mcp @{jsonrpc='2.0';id=2;method='resources/read';params=@{uri=$resource}}).Content
    }
    (Send-Mcp @{jsonrpc='2.0';id=3;method='tools/call';params=@{name='read_console';arguments=@{action='get';types=@('error');count=30;format='detailed'}}}).Content
}
