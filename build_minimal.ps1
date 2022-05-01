$ErrorActionPreference = 'Stop'

$visualStudioInstallation = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -property installationPath
$configuration = 'Release'

$msbuild = Join-Path $visualStudioInstallation 'MSBuild\Current\Bin\MSBuild.exe'
& $msbuild /t:Build /restore /v:minimal /p:Configuration=$configuration
