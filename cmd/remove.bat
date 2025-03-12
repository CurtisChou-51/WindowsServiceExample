set service_name=WindowsServiceExample
set service_install_path=%ProgramFiles%\WindowsServiceExample

sc.exe stop "%service_name%"
sc.exe delete "%service_name%"
rmdir /S /Q "%service_install_path%"

pause
goto :eof