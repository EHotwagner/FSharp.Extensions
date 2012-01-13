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

if NOT "%FSC:NOTAVAIL=X%" == "%FSC%" ( 
  REM Skipping test for FSI.EXE
  goto Skip
)

REM **************************

REM only a valid test if generics supported

  "%FSC%" %fsc_flags% -o:collections.exe -g collections.fs
  @if ERRORLEVEL 1 goto Error

  "%PEVERIFY%" collections.exe
  @if ERRORLEVEL 1 goto Error


REM **************************

REM only a valid test if generics supported

  %CSC% /nologo /target:library /out:classes-lib.dll classes.cs 
  @if ERRORLEVEL 1 goto Error

  "%FSC%" %fsc_flags% -r:classes-lib.dll -o:classes.exe -g classes.fs
  @if ERRORLEVEL 1 goto Error

  "%PEVERIFY%" classes.exe
  @if ERRORLEVEL 1 goto Error




REM **************************

REM only a valid test if generics supported
  %CSC% /nologo /target:library /out:byrefs-lib.dll byrefs.cs 
  @if ERRORLEVEL 1 goto Error

  "%FSC%" %fsc_flags% -r:byrefs-lib.dll -o:byrefs.exe -g byrefs.fs
  @if ERRORLEVEL 1 goto Error

  "%PEVERIFY%" byrefs.exe
  @if ERRORLEVEL 1 goto Error

REM **************************

%CSC% /nologo /target:library /out:methods-lib.dll methods.cs 
@if ERRORLEVEL 1 goto Error

"%FSC%" %fsc_flags% -r:methods-lib.dll -o:methods.exe -g methods.fs
@if ERRORLEVEL 1 goto Error

  "%PEVERIFY%" methods.exe
  @if ERRORLEVEL 1 goto Error


REM **************************

%CSC% /nologo /target:library /out:events-lib.dll events.cs 
@if ERRORLEVEL 1 goto Error

"%FSC%" %fsc_flags% --progress -r:events-lib.dll -o:events.exe -g events.fs
@if ERRORLEVEL 1 goto Error

  "%PEVERIFY%" events.exe
  @if ERRORLEVEL 1 goto Error


REM **************************

%CSC% /nologo /target:library /out:indexers-lib.dll indexers.cs 
@if ERRORLEVEL 1 goto Error

"%FSC%" %fsc_flags% -r:indexers-lib.dll -o:indexers.exe -g indexers.fs
@if ERRORLEVEL 1 goto Error

  "%PEVERIFY%" indexers.exe
  @if ERRORLEVEL 1 goto Error


REM **************************

%CSC% /nologo /target:library /out:fields-lib.dll fields.cs 
@if ERRORLEVEL 1 goto Error

"%FSC%" %fsc_flags% -r:fields-lib.dll -o:fields.exe -g fields.fs
@if ERRORLEVEL 1 goto Error

  "%PEVERIFY%" fields.exe 
  @if ERRORLEVEL 1 goto Error

REM **************************

%CSC% /nologo /target:library /out:properties-lib.dll properties.cs 
@if ERRORLEVEL 1 goto Error

"%FSC%" %fsc_flags% -r:properties-lib.dll -o:properties.exe -g properties.fs
@if ERRORLEVEL 1 goto Error


  "%PEVERIFY%" properties.exe 
  @if ERRORLEVEL 1 goto Error

REM **************************
REM Excel interop

REM only a valid test if generics supported

REM Don't try to compile without Office installed, will fail verification on Dev10

set OfficeExists=0
IF EXIST "%SystemDrive%\Program Files\Microsoft Office\Office11" (set OfficeExists=1)
IF EXIST "%SystemDrive%\Program Files (x86)\Microsoft Office\Office11" (set OfficeExists=1)
IF EXIST "%SystemDrive%\Program Files\Microsoft Office\Office12" (set OfficeExists=1)
IF EXIST "%SystemDrive%\Program Files (x86)\Microsoft Office\Office12" (set OfficeExists=1)
IF EXIST "%SystemDrive%\Program Files\Microsoft Office\Office14" (set OfficeExists=1)
IF EXIST "%SystemDrive%\Program Files (x86)\Microsoft Office\Office14" (set OfficeExists=1)

IF %OfficeExists%==1 (
  "%FSC%" %fsc_flags% -r:Excel.dll -o:optional.exe -g optional.fs
  @if ERRORLEVEL 1 goto Error
   "%PEVERIFY%" optional.exe
  @if ERRORLEVEL 1 goto Error
)


REM **************************
REM Multi-module case

%CSC% /nologo /target:module /out:events-lib.netmodule events.cs 
@if ERRORLEVEL 1 goto Error
%CSC% /nologo /target:module /out:properties-lib.netmodule properties.cs 
@if ERRORLEVEL 1 goto Error
%CSC% /nologo /target:module /out:fields-lib.netmodule fields.cs 
@if ERRORLEVEL 1 goto Error
%CSC% /nologo /target:module /out:indexers-lib.netmodule indexers.cs 
@if ERRORLEVEL 1 goto Error
"%ALINK%" /target:library /out:multi-module-lib.dll events-lib.netmodule properties-lib.netmodule fields-lib.netmodule indexers-lib.netmodule 
@if ERRORLEVEL 1 goto Error

"%FSC%" %fsc_flags% -r:multi-module-lib.dll -o:multi-module.exe -g properties.fs fields.fs indexers.fs events.fs 
@if ERRORLEVEL 1 goto Error

  "%PEVERIFY%" multi-module.exe
  @if ERRORLEVEL 1 goto Error


:Ok
echo Built fsharp %~n0 ok.
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

