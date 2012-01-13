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
set ERRORMSG=

call %~d0%~p0..\config.bat
if errorlevel 1 (
  set ERRORMSG=%ERRORMSG% config.bat failed;
  goto :ERROR
)

call :check "%FSC%"
if not "%ERRORMSG%"=="" goto :ERROR

%FSDIFF% %~f0 %~f0
@if ERRORLEVEL 1 (

    set ERRORMSG=%ERRORMSG% FSDIFF likely not found;
    goto Error
)


set testname=%1

set sources=
if exist "%testname%.mli" (set sources=%sources% %testname%.mli)
if exist "%testname%.fsi" (set sources=%sources% %testname%.fsi)
if exist "%testname%.ml" (set sources=%sources% %testname%.ml)
if exist "%testname%.fs" (set sources=%sources% %testname%.fs)
if exist "%testname%.fsx" (set sources=%sources% %testname%.fsx)
if exist "%testname%a.mli" (set sources=%sources% %testname%a.mli)
if exist "%testname%a.fsi" (set sources=%sources% %testname%a.fsi)
if exist "%testname%a.ml" (set sources=%sources% %testname%a.ml)
if exist "%testname%a.fs" (set sources=%sources% %testname%a.fs)
if exist "%testname%b.mli" (set sources=%sources% %testname%b.mli)
if exist "%testname%b.fsi" (set sources=%sources% %testname%b.fsi)
if exist "%testname%b.ml" (set sources=%sources% %testname%b.ml)
if exist "%testname%b.fs" (set sources=%sources% %testname%b.fs)

REM check negative tests for bootstrapped fsc.exe due to line-ending differences
if "%FSC:fscp=X%" == "%FSC%" ( 

  echo Negative typechecker testing: %testname%
  echo "%FSC%" %fsc_flags% --warnaserror --nologo --maxerrors:10000 -a -o:%testname%.dll  %sources%
  "%FSC%" %fsc_flags% --warnaserror --nologo --maxerrors:10000 -a -o:%testname%.dll  %sources% 2> %testname%.err
  @if NOT ERRORLEVEL 1 (
    set ERRORMSG=%ERRORMSG% FSC passed unexpectedly for  %sources%;
    goto SetError
  )

  %FSDIFF% %testname%.err %testname%.bsl > %testname%.diff
  for /f %%c IN (%testname%.diff) do (
    echo ***** %testname%.err %testname%.bsl differed: a bug or baseline may neeed updating
    set ERRORMSG=%ERRORMSG% %testname%.err %testname%.bsl differ;

    IF DEFINED WINDIFF  (start %windiff% %testname%.bsl  %testname%.err)
    goto SetError 

  )
  echo Good, output %testname%.err matched %testname%.bsl
)

:Ok
echo Ran fsharp %~f0 ok.
endlocal
exit /b 0
goto :EOF

:Skip
echo Skipped %~f0
endlocal
exit /b 0
goto :EOF


:Error
call %SCRIPT_ROOT%\ChompErr.bat %ERRORLEVEL% %~f0  "%ERRORMSG%"
exit /b %ERRORLEVEL% 
goto :EOF


:SETERROR
set NonexistentErrorLevel 2> nul
goto Error
goto :EOF


:Check
for /f  %%i in ("%1") do (
  dir %%i > NUL 2>&1 || (
    set ERRORMSG=%ERRORMSG% %1 was not found; 
  )
)
goto :EOF


