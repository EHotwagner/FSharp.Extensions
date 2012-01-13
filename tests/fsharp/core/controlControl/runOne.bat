set CONTROL_FAILURES_LOG=%~dp0\failures.log
echo %1: >> %CONTROL_FAILURES_LOG%
cd ..\%1
call run.bat
set X=%ERRORLEVEL%
cd ..\controlControl
exit /b %X%
