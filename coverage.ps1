$ocpath = Join-Path $HOME .nuget\packages\OpenCover\4.7.922\tools\OpenCover.Console.exe
& $ocpath -register:user -target:"dotnet.exe" -targetargs:"test --no-build --configuration Debug" -output:".\coverage.xml" -filter:"+[SymbolicExecution*]* -[SymbolicExecution.Test*]*"
Invoke-WebRequest -Uri 'https://codecov.io/bash' -OutFile codecov.sh
bash codecov.sh -f "coverage.xml" -t 992788fa-0d03-4f07-8b6d-2ae6fde6c6cd
