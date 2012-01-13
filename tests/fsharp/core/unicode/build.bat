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

call %~d0%~p0..\..\..\config.bat
@if ERRORLEVEL 1 goto Error

if NOT "%FSC:NOTAVAIL=X%" == "%FSC%" ( 
  REM Skipping test for FSI.EXE
  goto Skip
)

call %~d0%~p0..\..\single-test-build.bat
@if ERRORLEVEL 1 goto Error

REM just checking the files actually parse/compile for now....

"%FSC%" %fsc_flags% -a -o:kanji-unicode-utf8-nosig-codepage-65001.dll -g kanji-unicode-utf8-nosig-codepage-65001.fs
@if ERRORLEVEL 1 goto Error

"%FSC%" %fsc_flags% -a -o:kanji-unicode-utf8-nosig-codepage-65001.dll -g kanji-unicode-utf8-nosig-codepage-65001.fs
@if ERRORLEVEL 1 goto Error

REM check non-utf8 and --codepage flag for bootstrapped fsc.exe
if NOT "%FSC:fscp=X%" == "%FSC%" ( 
  "%FSC%" %fsc_flags% -a -o:kanji-unicode-utf16.dll -g kanji-unicode-utf16.fs
  @if ERRORLEVEL 1 goto Error

  "%FSC%" %fsc_flags% -a --codepage:65000 -o:kanji-unicode-utf7-codepage-65000.dll -g kanji-unicode-utf7-codepage-65000.fs
  @if ERRORLEVEL 1 goto Error

)
"%FSC%" %fsc_flags% -a -o:kanji-unicode-utf8-withsig-codepage-65001.dll -g kanji-unicode-utf8-withsig-codepage-65001.fs
@if ERRORLEVEL 1 goto Error


:Ok
echo Built fsharp %~f0 ok.
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

