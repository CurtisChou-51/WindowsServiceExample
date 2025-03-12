@echo off
cd /d %~dp0

set service_name=WindowsServiceExample
set service_install_path=%ProgramFiles%\WindowsServiceExample
set service_bin_path=%service_install_path%\WindowsServiceExample.exe

sc.exe query "%service_name%" | find /i "%service_name%" >install.log
if not errorlevel 1 (goto exist) else (goto notexist)

:exist
sc.exe query | find /i "%service_name%" >>install.log
if not errorlevel 1 (goto active) else (goto inactive)

:active
sc.exe stop "%service_name%" >>install.log

:inactive
md "%service_install_path%" >>install.log
xcopy publish "%service_install_path%" /S /Y >>install.log
sc.exe start "%service_name%" >>install.log
pause
goto :eof

:notexist
md "%service_install_path%" >>install.log
xcopy publish "%service_install_path%" /S /Y >>install.log
sc.exe create "%service_name%" binPath= "%service_bin_path%" start= auto >>install.log
sc.exe start "%service_name%" >>install.log
pause
goto :eof