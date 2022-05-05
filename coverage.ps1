$ocpath = Join-Path $HOME .nuget\packages\OpenCover\4.7.922\tools\OpenCover.Console.exe
& $ocpath -register:user -target:"dotnet.exe" -targetargs:"test --no-build --configuration Debug" -output:".\coverage.xml" -filter:"+[SymbolicExecution*]* -[SymbolicExecution.Test*]*"
$ProgressPreference = 'SilentlyContinue'
Invoke-WebRequest -Uri https://uploader.codecov.io/latest/windows/codecov.exe -OutFile codecov.sh
.\codecov.sh -f "coverage.xml"
