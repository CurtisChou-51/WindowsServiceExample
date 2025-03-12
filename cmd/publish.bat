@echo off
setlocal

set project_path=..\WindowsServiceExample
set publish_path=D:\WindowsServiceExample_Publish\WindowsServiceExample
set configuration=Release

dotnet publish %project_path% -c %configuration% -o %publish_path%
copy install.bat %publish_path%\..
copy remove.bat %publish_path%\..

endlocal
pause
