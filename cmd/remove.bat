set sname=WindowsServiceExample
set spath=%ProgramFiles%

sc.exe stop "%sname%"
sc.exe delete "%sname%"
rmdir /S /Q "%spath%\publish"

pause
goto :eof