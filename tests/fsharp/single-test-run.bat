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

dir build.ok > NUL ) || (
  @echo 'build.ok' not found.
  set ERRORMSG=%ERRORMSG% Skipped because 'build.ok' not found.
  goto :ERROR
)


call %~d0%~p0..\config.bat
if errorlevel 1 (
  set ERRORMSG=%ERRORMSG% config.bat failed;
  goto :ERROR
)

REM NOTE: There is an expectation here that the full path to FSC, FSi and other tools be given.  
REM This is reasonable for testing, because you want to know exactly which binary you are running.
REM Ideally we would use 'where' here, but WinXP does not support that.
REM Trying do define my own WHERE

call :WHERE %FSC%
if errorlevel 1 goto :ERROR
call :WHERE %FSI%
if errorlevel 1 goto :ERROR
for /f  %%i in ("%PERF%") do call :WHERE %%i
if errorlevel 1 goto :ERROR


set sources=
if exist testlib.fsi (set sources=%sources% testlib.fsi)
if exist testlib.fs (set sources=%sources% testlib.fs)
if exist test.mli (set sources=%sources% test.mli)
if exist test.ml (set sources=%sources% test.ml)
if exist test.fsi (set sources=%sources% test.fsi)
if exist test.fs (set sources=%sources% test.fs)
if exist test2.mli (set sources=%sources% test2.mli)
if exist test2.ml (set sources=%sources% test2.ml)
if exist test2.fsi (set sources=%sources% test2.fsi)
if exist test2.fs (set sources=%sources% test2.fs)
if exist test.fsx (set sources=%sources% test.fsx)
if exist test2.fsx (set sources=%sources% test2.fsx)

set sourceshw=
if exist test-hw.mli (set sourceshw=%sourceshw% test-hw.mli)
if exist test-hw.ml (set sourceshw=%sourceshw% test-hw.ml)
if exist test2-hw.mli (set sourceshw=%sourceshw% test2-hw.mli)
if exist test2-hw.ml (set sourceshw=%sourceshw% test2-hw.ml)

:START
REM =========================================
call :FSI
call :FSC_BASIC
call :FSC_HW
call :FSC_O3
call :PERF_O3
REM call :FSC_STANDALONE
call :NGEN
call :GENERATED_SIGNATURE
call :EMPTY_SIGNATURE
call :FRENCH
call :SPANISH

if "%ERRORMSG%"==""  goto Ok

set NonexistentErrorLevel 2> nul
goto :ERROR
REM =========================================
:END


:EXIT_PATHS
REM =========================================

:Ok
echo Ran fsharp %~f0 ok.
exit /b 0
goto :EOF

:Skip
echo Skipped %~f0
exit /b 0
goto :EOF

:Error
call %SCRIPT_ROOT%\ChompErr.bat %ERRORLEVEL% %~f0  "%ERRORMSG%"
exit /b %ERRORLEVEL% 
goto :EOF



REM =========================================
REM THE TESTS
REM =========================================

:FSI
@echo do :FSI
REM =========================================
if NOT EXIST dont.pipe.to.stdin (
  if exist test.ok (del /f /q test.ok)
  %CLIX% "%FSI%" %fsi_flags% < %sources% && (
  dir test.ok > NUL 2>&1 ) || (
  @echo :FSI failed;
  set ERRORMSG=%ERRORMSG% FSI failed;
  )
)

if NOT EXIST dont.pipe.to.stdin (
  if exist test.ok (del /f /q test.ok)
  %CLIX% "%FSI%" %fsi_flags% --optimize < %sources% && (
  dir test.ok > NUL 2>&1 ) || (
  @echo :FSI --optimize failed
  set ERRORMSG=%ERRORMSG% FSI --optimize failed;
  )
)

if NOT EXIST dont.pipe.to.stdin (
  if exist test.ok (del /f /q test.ok)
  %CLIX% "%FSI%" %fsi_flags% --gui < %sources% && (
  dir test.ok > NUL 2>&1 ) || (
  @echo :FSI --gui failed;
  set ERRORMSG=%ERRORMSG% FSI --gui failed;
  )
)

  REM If redirection failed, don't try loading the script
  REM since fsi will stop on error, and the run will hang.
  if not "%ERRORMSG%"==""  (
    @echo Remaining FSI skipped...    
    set ERRORMSG=%ERRORMSG% Remaining FSI skipped...;    
    goto :EOF
  )

if NOT EXIST dont.pipe.to.stdin (
  if exist test.ok (del /f /q test.ok)
  %CLIX% "%FSI%" %fsi_flags% %sources% && (
  dir test.ok > NUL 2>&1 ) || (
  @echo :FSI load failed
  set ERRORMSG=%ERRORMSG% FSI load failed;
  )
)
  goto :EOF

:FSC_BASIC
@echo do :FSC_BASIC
REM =========================================
  if exist test.ok (del /f /q test.ok)
  %CLIX% .\test.exe && (
  dir test.ok > NUL 2>&1 ) || (
  @echo :FSC_BASIC failed
  set ERRORMSG=%ERRORMSG% FSC_BASIC failed;
  )
goto :EOF

:FSC_HW
@echo do :FSC_HW
REM =========================================
if exist test-hw.ml (
  if exist test.ok (del /f /q test.ok)
  %CLIX% .\test-hw.exe && (
  dir test.ok > NUL 2>&1 ) || (
  @echo  :FSC_HW failed
  set ERRORMSG=%ERRORMSG% FSC_HW failed;
  )
)
goto :EOF

:FSC_O3
@echo do :FSC_O3
REM =========================================
  if exist test.ok (del /f /q test.ok)
  %CLIX% .\test--optimize.exe && (
  dir test.ok > NUL 2>&1 ) || (
  @echo :FSC_O3 failed
  set ERRORMSG=%ERRORMSG% FSC_03 failed;
  )
goto :EOF

:PERF_O3
IF NOT DEFINED PERF GOTO :EOF
@echo do :PERF_O3
REM =========================================
  if exist test.ok (del /f /q test.ok)
  %PERF% %CLIX% .\test--optimize.exe && (
  dir test.ok > NUL 2>&1 ) || (
  @echo :PERF_O3 failed
  set ERRORMSG=%ERRORMSG% PERF_O3 failed;
  )
goto :EOF

:FSC_STANDALONE
@echo do :FSC_STANDALONE
REM =========================================
REM if exist test.ok (del /f /q test.ok)
REM %CLIX% .\test--optimize-standalone.exe
REM if ERRORLEVEL 1 goto Error
REM if NOT EXIST test.ok goto SetError
REM goto :EOF


:NGEN
@echo do :NGEN
REM =========================================
if exist test.ok (del /f /q test.ok)
if exist ngen (rmdir /s /q ngen)

REM add logging to trace elevator issue in lab
if defined NGEN (
  echo.
  echo LOG:  NGEN=%NGEN%.  Uninstall existing exe's and look for new exe.
  echo LOG:  call   "%NGEN%" uninstall .\ngen\test--optimize.exe  redirect to NUL
  %ADMIN_PIPE% "%NGEN%" uninstall .\ngen\test--optimize.exe  > NUL
  dir .\test--optimize.exe > NUL 2>&1 ) && (
  echo LOG:  Found .\test--optimize.exe. do Mkdir ngen.
  mkdir ngen
  echo LOG:  made dir ngen. Do Copy exe and pdb

  if exist cslib.dll (copy /y cslib.dll .\ngen)
  copy /y .\test--optimize.exe .\ngen
  copy /y .\test--optimize.pdb .\ngen
  copy /y .\*.dll .\ngen

  echo LOG:  copied .exe and .pdb to .\ngen. now do ngen.
  echo LOG:  call "%NGEN%" .\ngen\test--optimize.exe
  %ADMIN_PIPE% "%NGEN%" .\ngen\test--optimize.exe) && (
  echo LOG:  NGEN succeeded. Execute .exe
  %CLIX% .\ngen\test--optimize.exe) && (
  echo LOG:  exe ran. Look for test.ok
  dir test.ok > NUL 2>&1 ) && (
  echo LOG:  NGEN phase succeeded. ) || (
  @echo :NGEN failed
  set ERRORMSG=%ERRORMSG% NGEN failed;
  echo LOG:  NGEN phase failed.
  )



)
goto :EOF

rem if NOT "%NGEN%"=="" (
rem   if exist ngen-standalone (rmdir /s /q ngen-standalone)
rem   mkdir ngen-standalone
rem 
rem   copy /y .\test--optimize-standalone.exe .\ngen-standalone
rem   copy /y .\test--optimize-standalone.pdb .\ngen-standalone
rem 
rem   "%NGEN%" .\ngen-standalone\test--optimize-standalone.exe
rem   if ERRORLEVEL 1 goto Error
rem 
rem   if exist test.ok (del /f /q test.ok)
rem   .\ngen-standalone\test--optimize-standalone.exe
rem   if ERRORLEVEL 1 goto Error
rem   if NOT EXIST test.ok goto SetError
rem 
rem )
rem if ERRORLEVEL 1 goto Error

rem c:\clrprofiler\Binaries\CLRProfiler.exe  -p .\test--optimize-standalone.exe


:GENERATED_SIGNATURE
@echo do :GENERATED_SIGNATURE
REM =========================================
if NOT EXIST dont.use.generated.signature (
  if exist test.ml (
    if exist test.ok (del /f /q test.ok)
    %CLIX% tmptest1.exe && (
    dir test.ok > NUL 2>&1 ) || (
    @echo :GENERATED_SIGNATURE failed
    set ERRORMSG=%ERRORMSG% FSC_GENERATED_SIGNATURE failed;
    )
  )
)
goto :EOF

:EMPTY_SIGNATURE
@echo do :EMPTY_SIGNATURE
REM =========================================
if NOT EXIST dont.use.empty.signature (
  if exist test.ml (

    if exist test.ok (del /f /q test.ok)
    %CLIX% tmptest2.exe && (
    dir test.ok > NUL 2>&1 ) || (
    @echo :EMPTY_SIGNATURE failed
    set ERRORMSG=%ERRORMSG% FSC_EMPTY_SIGNATURE failed;
    )

    if exist test.ok (del /f /q test.ok)
    %CLIX% tmptest2--optimize.exe && (
      dir test.ok > NUL 2>&1 ) || (
      @echo :EMPTY_SIGNATURE --optimize failed
      set ERRORMSG=%ERRORMSG% FSC_EMPTY_SIGNATURE --optimize failed;
    )
  )
)
goto :EOF

:FRENCH
@echo do :FRENCH
REM =========================================
  if exist test.ok (del /f /q test.ok)
  %CLIX% .\test.exe fr-FR && (
  dir test.ok > NUL 2>&1 ) || (
  @echo :FRENCH failed
  set ERRORMSG=%ERRORMSG% FRENCH failed;
  )
goto :EOF

:SPANISH
@echo do :SPANISH
REM =========================================
  if exist test.ok (del /f /q test.ok)
  %CLIX% .\test.exe es-ES && (
  dir test.ok > NUL 2>&1 ) || (
  @echo :SPANISH failed
  set ERRORMSG=%ERRORMSG% SPANISH failed;
  )
goto :EOF


:WHERE
for /f %%i in ("%~1") do set _WHERE=%%~$PATH:i
if not defined _WHERE set ERRORMSG=%ERRORMSG% %~1 not found;
set _WHERE > NUL 2>&1
goto :EOF
