@echo off
publish New HandSupportLib.ImageProcessing package to Github.com
color 1b
echo Command Executer by: Mohammed Alsailamy
echo #################################
SET /P UpdateApiKey=Do you Want Update ApiKey (Y/[N])?
IF /I "%UpdateApiKey%" NEQ "Y" GOTO END
echo Start Updating GITHUB Token Key
set /p GITHUB_TokenKey=Your github Token Key:
echo nuget config -set GITHUB_PACKAGES_TOKEN=%GITHUB_TokenKey%
nuget config -set GITHUB_PACKAGES_TOKEN=%GITHUB_TokenKey%
nuget sources add -name "github" -Source https://nuget.pkg.github.com/myalsailamy/index.json -Username nuget.pkg.github.com\myalsailamy -Password %GITHUB_TokenKey%
:END
echo #################################
REM delete old existing HandSupportLib.ImageProcessing build nuget packages
del bin\Debug\*.nupkg
msbuild /t:pack /p:Configuration=Debug
dotnet pack --configuration Debug
del bin\Release\*.nupkg
msbuild /t:pack /p:Configuration=Release
dotnet pack --configuration Release
echo #################################
for /f %%l in ('dir /b /s bin\Release\*.nupkg') do (
   dotnet nuget push %%l --source https://nuget.pkg.github.com/myalsailamy/index.json --api-key e9e5ed1e3ca3f642ac743f740812397a5f696c20 --skip-duplicate 
   REM // OR push With the saved api-key
   REM nuget push %%l -source https://nuget.pkg.github.com/myalsailamy/index.json 
)
echo ################ Done #################
PAUSE
