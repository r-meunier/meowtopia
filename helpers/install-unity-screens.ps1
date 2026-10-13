param([string]$ProjectPath = (Join-Path $PSScriptRoot '..\meowtopia-unity'))
$ErrorActionPreference = 'Stop'
$project = (Resolve-Path -LiteralPath $ProjectPath).Path
$scripts = Join-Path $project 'Assets\Scripts\Screens'
$editor = Join-Path $project 'Assets\Editor'
New-Item -ItemType Directory -Force -Path $scripts,$editor | Out-Null
foreach ($name in @('MeowtopiaUI.cs','MeowtopiaLoadingScreen.cs','MeowtopiaHomeScreen.cs','MeowtopiaAnimatedCat.cs','MeowtopiaEyeOverlay.cs','MeowtopiaGazeEye.cs','MeowtopiaDraggableToy.cs','MeowtopiaToyGraphic.cs','MeowtopiaPlaySession.cs')) {
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot "unity-screen-staging\$name") -Destination (Join-Path $scripts $name)
}
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'unity-screen-staging\MeowtopiaScreenSetup.cs') -Destination (Join-Path $editor 'MeowtopiaScreenSetup.cs')
$ragdollSource = Join-Path $PSScriptRoot '..\resources\Assets\Ragdoll\RagdollIdle-v2.png'
if (Test-Path -LiteralPath $ragdollSource) {
    $art = Join-Path $project 'Assets\Art\Pets\Ragdoll'
    New-Item -ItemType Directory -Force -Path $art | Out-Null
    Copy-Item -LiteralPath $ragdollSource -Destination (Join-Path $art 'RagdollIdle.png')
}
Write-Output 'Installed loading and home screen scripts in the active Unity project.'
