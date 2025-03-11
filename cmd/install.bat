@echo off
cd /d %~dp0

set sname=WindowsServiceExample
set spath=%ProgramFiles%

sc.exe query "%sname%" | find /i "%sname%" >install.log
if not errorlevel 1 (goto exist) else (goto notexist)

:exist
sc.exe query | find /i "%sname%" >>install.log
if not errorlevel 1 (goto active) else (goto inactive)

:active
sc.exe stop "%sname%" >>install.log

:inactive
md "%spath%\publish" >>install.log
xcopy publish "%spath%\publish" /S /Y >>install.log
sc.exe start "%sname%" >>install.log
pause
goto :eof

:notexist
md "%spath%\publish" >>install.log
xcopy publish "%spath%\publish" /S /Y >>install.log
sc.exe create "%sname%" binPath= "%spath%\publish\WindowsServiceExample.exe" start= auto >>install.log
sc.exe start "%sname%" >>install.log
pause
goto :eof