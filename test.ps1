$ErrorActionPreference = 'Stop'

$visualStudioInstallation = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -property installationPath
$configuration = 'Release'
$artifactsDir = Join-Path $PSScriptRoot 'artifacts'

$msbuild = Join-Path $visualStudioInstallation 'MSBuild\Current\Bin\MSBuild.exe'
# Build and test solution
& $msbuild /t:Build /restore /v:minimal /p:Configuration=$configuration
dotnet test --no-build --configuration $configuration
# Ensure a debug build is made for tests
if ($configuration -ne 'Debug') {
    & $msbuild /t:Build /restore /v:minimal /p:Configuration='Debug'
    dotnet test --no-build --configuration Debug
}
