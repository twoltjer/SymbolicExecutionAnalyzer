$ErrorActionPreference = 'Stop'

echo "Starting build"

$visualStudioInstallation = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -property installationPath
$configuration = 'Release'
$artifactsDir = Join-Path $PSScriptRoot 'artifacts'

echo "Join-Path"
dir

$msbuild = Join-Path $visualStudioInstallation 'MSBuild\Current\Bin\MSBuild.exe'
& $msbuild /t:Build /restore /v:minimal /p:Configuration=$configuration
& $msbuild SymbolicExecution /t:Pack /p:NoBuild=true /p:PackageOutputPath="$artifactsDir" /v:minimal /p:Configuration=$configuration

echo "Copy-Item"

Copy-Item SymbolicExecution.Vsix\bin\$configuration\SymbolicExecution.vsix $artifactsDir
