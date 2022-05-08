$ocpath = Join-Path $HOME .nuget\packages\OpenCover\4.7.922\tools\OpenCover.Console.exe
& $ocpath -register:user -target:"dotnet.exe" -targetargs:"test --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=opencover" -output:".\coverage.xml" -filter:"+[SymbolicExecution*]* -[SymbolicExecution.Test*]*"
$ProgressPreference = 'SilentlyContinue'
Invoke-WebRequest -Uri https://uploader.codecov.io/latest/windows/codecov.exe -OutFile codecov.exe
.\codecov.exe -f "coverage.xml"
