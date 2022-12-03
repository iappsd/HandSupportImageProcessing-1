@echo off

publish New HandSupportLib.ImageProcessing package 
color 1b
echo Command Executer by: Mohammed Alsailamy
echo #################################
REM delete existing HandSupportLib.ImageProcessing nuget packages
del bin\Debug\*.nupkg
msbuild /t:pack /p:Configuration=Debug
dotnet pack --configuration Debug
del bin\Release\*.nupkg
msbuild /t:pack /p:Configuration=Release
dotnet pack --configuration Release
echo #################################
echo Export To (D:\WebSiteBuilders\source\nuget_repo)
REM ** Push the file to myget ** 
REM There should only be a single file in the 
for /f %%l in ('dir /b /s bin\Release\*.nupkg') do (
	nuget add %%l -source D:\WebSiteBuilders\source\nuget_repo
)
echo ################ Done #################
PAUSE
