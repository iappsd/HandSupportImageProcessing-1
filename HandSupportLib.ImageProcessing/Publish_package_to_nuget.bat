@echo off
publish New HandSupportLib.ImageProcessing package to www.nuget.org
color 1b
echo Command Executer by: Mohammed Alsailamy
SET /P UpdateApiKey=Do you Want Update ApiKey (Y/[N])?
IF /I "%UpdateApiKey%" NEQ "Y" GOTO END
echo Start Updating ApiKey
set /p ApiKey=Your ApiKey:
echo nuget setApiKey %ApiKey%
nuget setApiKey %ApiKey%
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
   dotnet nuget push %%l --source https://api.nuget.org/v3/index.json
)
echo ################ Done #################
PAUSE
