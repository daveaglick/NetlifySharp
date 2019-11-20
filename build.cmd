@echo off
cd "build\NetlifySharp.Build"
dotnet run -- %*
set exitcode=%errorlevel%
cd %~dp0
exit /b %exitcode%