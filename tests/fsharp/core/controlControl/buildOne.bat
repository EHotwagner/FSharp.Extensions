cd ..\%1
call build.bat
set X=%ERRORLEVEL%
cd ..\controlControl
exit /b %X%