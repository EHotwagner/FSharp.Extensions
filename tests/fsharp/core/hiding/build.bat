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
REM Configure the sample, i.e. where to find the F# compiler and C# compiler.
if EXIST build.ok DEL /f /q build.ok

call %~d0%~p0..\..\..\config.bat
@if ERRORLEVEL 1 goto Error

if NOT "%FSC:NOTAVAIL=X%" == "%FSC%" ( 
  ECHO Skipping test for FSI.EXE
  goto Skip
)


"%FSC%" %fsc_flags% -a --optimize -o:lib.dll lib.mli lib.ml libv.ml
@if ERRORLEVEL 1 goto Error

"%PEVERIFY%" lib.dll
@if ERRORLEVEL 1 goto Error


"%FSC%" %fsc_flags% -a --optimize -r:lib.dll -o:lib2.dll lib2.mli lib2.ml lib3.ml
@if ERRORLEVEL 1 goto Error

"%PEVERIFY%" lib2.dll
@if ERRORLEVEL 1 goto Error


"%FSC%" %fsc_flags% --optimize -r:lib.dll -r:lib2.dll -o:client.exe client.ml
@if ERRORLEVEL 1 goto Error

"%PEVERIFY%" client.exe
@if ERRORLEVEL 1 goto Error



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
call %SCRIPT_ROOT%\ChompErr.bat %ERRORLEVEL% %~f0
endlocal
exit /b %ERRORLEVEL%

