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
rem there is no build.bat for this testcase, so don't check for build.ok

call %~d0%~p0..\..\..\config.bat

  if exist test.ok (del /f /q test.ok)
  "%FSI%" %fsi_flags%  --maxerrors:1 < test1.ml
  if NOT EXIST test.ok goto SetError

:Ok
echo Ran fsharp %~f0 ok.
endlocal
exit /b 0

:Skip
echo Skipped %~f0
endlocal
exit /b 0


:Error
call %SCRIPT_ROOT%\ChompErr.bat 1 %~f0
endlocal
exit /b %ERRORLEVEL%

:SETERROR
set NonexistentErrorLevel 2> nul
goto Error
