@if "%_echo%"=="" echo off

:SETPATHS
set _SCRIPT_DRIVE=%~d0
set _SCRIPT_PATH=%~p0
set SCRIPT_ROOT=%_SCRIPT_DRIVE%%_SCRIPT_PATH%

:SETUNATTENDED
if not "%1"=="" set _UNATTENDEDLOG=%~f1

if not DEFINED _UNATTENDEDLOG goto SETCONFIGS

REM if exist "%_UNATTENDEDLOG%" del /f /q %_UNATTENDEDLOG%
REM Append to the file, if it alredy exists.
echo started %~f0 >> %_UNATTENDEDLOG%
date /t >> %_UNATTENDEDLOG%
time /t >> %_UNATTENDEDLOG%
echo.  >> %_UNATTENDEDLOG%

if not defined WINDIFF goto SETCONFIGS
if not exist %WINDIFF% (
  for /f "usebackq" %%f in ('%WINDIFF%') do @if not "%%~$PATH:f"=="" set WINDIFF=%%~$PATH:f
)
if not exist %WINDIFF% set WINDIFF=


:SETCONFIGS
setlocal
if exist "%SCRIPT_ROOT%..\..\setup\installed-ilx-configs" (
  for /f "tokens=1,2,3,4,5,6 delims=," %%i in (%SCRIPT_ROOT%..\..\setup\installed-ilx-configs) do ( 
   if NOT "%%i"== "#" (
     set csc_flags=%%j
     set fsc_flags=%%k
     set fscroot=%%l
     set fsiroot=%%m
     set ILX_SUFFIX=%%n
   ) else (
     setErrorLevelByRunningNonExistentCommand  2> junk
     echo FAILED: No flags found in %SCRIPT_ROOT%..\..\setup\installed-ilx-configs
     goto Exit
   )
  )
) else (
  setErrorLevelByRunningNonExistentCommand  2> junk
  echo WARNING: Could not find %SCRIPT_ROOT%..\..\setup\installed-ilx-configs
  echo WARNING: Continuing assuming this is a non-dev setup.
)

:BUILDANDRUN
call %SCRIPT_ROOT%\build-and-run.bat
if ERRORLEVEL 1 goto REPORTFAILURES
exit /b 0

:REPORTFAILURES
if not DEFINED _UNATTENDEDLOG goto Exit

echo =====================================================
echo.
echo.
echo %~f0 completed 
echo.
echo LogFile and # of failures = 
find /c "ERRORLEVEL" %_UNATTENDEDLOG%
echo (# of failures may be cummulative)
echo.
echo.
echo =====================================================


echo completed %~f0 >> %_UNATTENDEDLOG%
date /t >> %_UNATTENDEDLOG%
time /t >> %_UNATTENDEDLOG%
echo ===================================================== >> %_UNATTENDEDLOG%
echo. >> %_UNATTENDEDLOG%

:Exit
endlocal
if DEFINED _UNATTENDEDLOG (
  set _UNATTENDEDLOG=
  REM call config.bat one more time, just to echo the paths to the binaries
  call %SCRIPT_ROOT%\config.bat
)
exit /b %ERRORLEVEL%
