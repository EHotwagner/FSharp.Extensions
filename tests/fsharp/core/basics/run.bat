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
if ERRORLEVEL 1 goto Error

if not exist "%ILDASM%" (
   @echo '%ILDASM%' not found.
    goto Error 
)

if "%SSCLI%" == "true" (goto Skip)

"%ILDASM%" /nobar /out=test.il test.exe

"%ILDASM%" /nobar /out=test--optimize.il test--optimize.exe

type test--optimize.il | find /C "ShouldNotAppear" > count--optimize
type test.il | find /C "ShouldNotAppear" > count
for /f %%c IN (count--optimize) do (if NOT "%%c"=="0" (
   echo Error: optimizations not removed.  Relevant lines from IL file follow:
   type test--optimize.il | find "ShouldNotAppear"
   goto SetError)
)
for /f %%c IN (count) do (
   set NUMELIM=%%c
)

:Ok
echo Ran fsharp %~f0 ok - optimizations removed %NUMELIM% textual occurrences of optimizable identifiers from target IL 
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

:SetError
set NonexistentErrorLevel 2> nul
goto Error
