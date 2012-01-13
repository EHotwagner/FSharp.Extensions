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
if EXIST build.ok DEL /f /q build.ok

call %~d0%~p0..\config.bat

if NOT "%FSC:NOTAVAIL=X%" == "%FSC%" ( 
  goto Skip
)

set source1=
if exist test.ml (set source1=test.ml)
if exist test.fs (set source1=test.fs)

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
if exist test-hw.fsx (set sourceshw=%sourceshw% test-hw.fsx)
if exist test2-hw.mli (set sourceshw=%sourceshw% test2-hw.mli)
if exist test2-hw.ml (set sourceshw=%sourceshw% test2-hw.ml)
if exist test2-hw.fsx (set sourceshw=%sourceshw% test2-hw.fsx)


if NOT EXIST dont.use.generated.signature (
 if exist test.ml (

  echo Generating interface file...
  copy /y %source1% tmptest.ml
  REM NOTE: use --generate-interface-file since results may be in Unicode
  "%FSC%" %fsc_flags% --sig:tmptest.mli tmptest.ml
  if ERRORLEVEL 1 goto Error  

  echo Compiling against generated interface file...
  "%FSC%" %fsc_flags% -o:tmptest1.exe tmptest.mli tmptest.ml
  if ERRORLEVEL 1 goto Error  

  if NOT EXIST dont.run.peverify (
    "%PEVERIFY%" tmptest1.exe
    @if ERRORLEVEL 1 goto Error
  )
 )
)
if NOT EXIST dont.use.empty.signature (
  if exist test.ml ( 
    echo Compiling against empty interface file...
    echo // empty file  > tmptest2.mli

    copy /y %source1% tmptest2.ml
    "%FSC%" %fsc_flags% --define:COMPILING_WITH_EMPTY_SIGNATURE -o:tmptest2.exe tmptest2.mli tmptest2.ml
    if ERRORLEVEL 1 goto Error  


    if NOT EXIST dont.run.peverify (
  	  "%PEVERIFY%" tmptest2.exe
  	  @if ERRORLEVEL 1 goto Error
    ) 
      
    "%FSC%" %fsc_flags% --define:COMPILING_WITH_EMPTY_SIGNATURE --optimize -o:tmptest2--optimize.exe tmptest2.mli tmptest2.ml
    if ERRORLEVEL 1 goto Error  


    if NOT EXIST dont.run.peverify (
	  "%PEVERIFY%" tmptest2--optimize.exe
	  @if ERRORLEVEL 1 goto Error
     )

   )
)


"%FSC%" %fsc_flags% --define:BASIC_TEST -o:test.exe -g %sources%
if ERRORLEVEL 1 goto Error


if NOT EXIST dont.run.peverify (
	"%PEVERIFY%" test.exe
	@if ERRORLEVEL 1 goto Error
)

if exist test-hw.ml (
  "%FSC%" %fsc_flags% -o:test-hw.exe -g %sourceshw%
  if ERRORLEVEL 1 goto Error


  if NOT EXIST dont.run.peverify (
      "%PEVERIFY%" test-hw.exe
      @if ERRORLEVEL 1 goto Error
  )
)

"%FSC%" %fsc_flags% --optimize --define:PERF -o:test--optimize.exe -g %sources%
if ERRORLEVEL 1 goto Error


if NOT EXIST dont.run.peverify (
	"%PEVERIFY%" test--optimize.exe
	@if ERRORLEVEL 1 goto Error
)

REM Compile as a DLL to exercise pickling of interface data, then recompile the original source file referencing this DLL
REM THe second compilation will not utilize the information from the first in any meaningful way, but the
REM compiler will unpickle the interface and optimization data, so we test unpickling as well.

if NOT EXIST dont.compile.test.as.dll (
  "%FSC%" %fsc_flags% --optimize -a -o:test--optimize-lib.dll -g %sources%
  if ERRORLEVEL 1 goto Error
  "%FSC%" %fsc_flags% --optimize -r:test--optimize-lib.dll -o:test--optimize-client-of-lib.exe -g %sources%
  if ERRORLEVEL 1 goto Error


  if NOT EXIST dont.run.peverify (
	  "%PEVERIFY%" test--optimize-lib.dll
	  @if ERRORLEVEL 1 goto Error
  )
  

  if NOT EXIST dont.run.peverify (
      "%PEVERIFY%" test--optimize-client-of-lib.exe
  )
  @if ERRORLEVEL 1 goto Error
)

REM Compile using --standalone to tst that

REM "%FSC%" %fsc_flags% --optimize -o:test--optimize-standalone.exe --standalone -g %sources%
REM if ERRORLEVEL 1 goto Error

REM 
REM if NOT EXIST dont.run.peverify (
REM   "%PEVERIFY%" test--optimize-standalone.exe
REM   @if ERRORLEVEL 1 goto Error
REM )

if NOT EXIST dont.use.wrapper.namespace (
 if exist test.ml (

  echo Compiling when wrapped in a namespace declaration...

  echo module TestNamespace.TestModule > tmptest3.ml
  type %source1%  >> tmptest3.ml

  "%FSC%" %fsc_flags% -o:tmptest3.exe tmptest3.ml
  if ERRORLEVEL 1 goto Error  


  if NOT EXIST dont.run.peverify (
	  "%PEVERIFY%" tmptest3.exe
	  @if ERRORLEVEL 1 goto Error
  )
  
  "%FSC%" %fsc_flags% --optimize -o:tmptest3--optimize.exe tmptest3.ml
  if ERRORLEVEL 1 goto Error  


  if NOT EXIST dont.run.peverify (
	  "%PEVERIFY%" tmptest3--optimize.exe
	  @if ERRORLEVEL 1 goto Error
  )

 )
)

:Ok
echo Built fsharp %~f0 ok.
echo. > build.ok
endlocal
exit /b 0

:Skip
echo Skipped %~f0
endlocal
exit /b 0


:Error
echo Test Script Failed (perhaps test did not emit test.ok signal file?)
call %SCRIPT_ROOT%\ChompErr.bat %ERRORLEVEL% %~f0
endlocal
exit /b %ERRORLEVEL%

:SETERROR
set NonexistentErrorLevel 2> nul
goto Error
