@if "%_echo%"=="" echo off
REM ----------------------------------------------------------------------------
REM
REM Copyright (c) 2002-2011 Microsoft Corporation. 
REM
REM This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
REM copy of the license can be found in the License.html file at the root of this distribution. 
REM By using this source code in any fashion, you are agreeing to be bound 
REM by the terms of the Apache License, Version 2.0.
REM
REM You must not remove this notice, or any other, from this software.
REM ----------------------------------------------------------------------------

setlocal 
dir build.ok > NUL ) || (
  @echo 'build.ok' not found.
  goto :ERROR
)

call %~d0%~p0..\..\..\config.bat


REM **************************

  %CLIX% .\collections.exe
  if ERRORLEVEL 1 goto Error

REM **************************

%CLIX% .\events.exe
if ERRORLEVEL 1 goto Error


REM **************************

%CLIX% .\indexers.exe
if ERRORLEVEL 1 goto Error


REM **************************

%CLIX% .\fields.exe
if ERRORLEVEL 1 goto Error

REM **************************


  %CLIX% .\byrefs.exe
  if ERRORLEVEL 1 goto Error

REM **************************

%CLIX% .\methods.exe
if ERRORLEVEL 1 goto Error

REM **************************

%CLIX% .\properties.exe
if ERRORLEVEL 1 goto Error

REM **************************

  %CLIX% .\classes.exe
  if ERRORLEVEL 1 goto Error

:Ok
echo Ran fsharp %~f0 ok.
endlocal
exit /b 0

:Skip
echo Skipped %~f0
endlocal
exit /b 0


:Error
call %SCRIPT_ROOT%\ChompErr.bat %ERRORLEVEL% %~f0
endlocal
exit /b %ERRORLEVEL%
