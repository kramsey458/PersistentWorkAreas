param(
    [string]$GameDir = 'C:\Program Files (x86)\Steam\steamapps\common\Timberborn',
    [string]$OutputDir = (Join-Path $PSScriptRoot 'dist')
)
$ErrorActionPreference = 'Stop'
if (!(Test-Path -LiteralPath (Join-Path $GameDir 'Timberborn_Data\Managed\Timberborn.BuildingsNavigation.dll'))) {
    throw 'Timberborn game assemblies were not found. Supply -GameDir.'
}
dotnet build (Join-Path $PSScriptRoot 'source\PersistentWorkAreas.csproj') -c Release "-p:GameDir=$GameDir" -v minimal
if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }
$dll = Join-Path $PSScriptRoot 'source\bin\Release\netstandard2.1\PersistentWorkAreas.dll'
$assets = Join-Path $PSScriptRoot 'packaging\PersistentWorkAreas\version-1.1'
dotnet run --project (Join-Path $PSScriptRoot 'tests\Checks.csproj') -c Release -- $GameDir $dll $assets
if ($LASTEXITCODE -ne 0) { throw 'Validation failed.' }
$package = Join-Path $OutputDir 'PersistentWorkAreas'
$version = Join-Path $package 'version-1.1'
New-Item -ItemType Directory -Force $version | Out-Null
Copy-Item -Path (Join-Path $assets '*') -Destination $version -Recurse -Force
Copy-Item -LiteralPath $dll -Destination $version -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'README.md') -Destination $package -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'VALIDATION.md') -Destination $package -Force
$zip = Join-Path $OutputDir 'PersistentWorkAreas-v0.1.3.zip'
Compress-Archive -LiteralPath $package -DestinationPath $zip -Force
Get-FileHash -LiteralPath $zip -Algorithm SHA256
