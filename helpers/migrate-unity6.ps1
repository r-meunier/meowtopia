param([string]$ProjectPath = (Join-Path $PSScriptRoot '..\meowtopia-unity'))
$ErrorActionPreference = 'Stop'
$project = (Resolve-Path -LiteralPath $ProjectPath).Path
$samples = Join-Path $project 'Assets\TextMesh Pro\Examples & Extras\Scripts'
foreach ($file in (Get-ChildItem -LiteralPath $samples -Filter '*.cs')) {
    $original = [IO.File]::ReadAllText($file.FullName)
    $updated = $original.Replace('.enableWordWrapping = false', '.textWrappingMode = TextWrappingModes.NoWrap').Replace('.enableWordWrapping = true', '.textWrappingMode = TextWrappingModes.Normal')
    $updated = $updated.Replace('.enableKerning = false', '.fontFeatures.Clear()')
    # Clear() is a call, so the original assignment semicolon remains valid.
    $updated = [regex]::Replace($updated, 'Vector2\[\](\s+\w+\s*=\s*[^;]+\.uvs0\s*;)', 'Vector4[]$1')
    $updated = [regex]::Replace($updated, '(\w+(?:\.[\w]+|\[[^\]]+\])*\.mesh)\.uv\s*=\s*([^;]+\.uvs0);', '$1.SetUVs(0, $2);')
    if ($updated -ne $original) { [IO.File]::WriteAllText($file.FullName, $updated); Write-Output $file.Name }
}
$generator = Join-Path $project 'Assets\Scripts\UI\HomeRedesign\HomeCanvasGenerator.cs'
$original = [IO.File]::ReadAllText($generator)
$updated = $original.Replace('darkBgColor, Vector2.zero, Vector2.zero, Vector2.one, Vector2.one', 'darkBgColor, Vector2.zero, Vector2.one, Vector2.zero')
if ($updated -ne $original) { [IO.File]::WriteAllText($generator, $updated); Write-Output 'HomeCanvasGenerator.cs' }
