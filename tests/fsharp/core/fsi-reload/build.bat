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
goto ok

setlocal
REM Configure the sample, i.e. where to find the F# compiler and C# compiler.

call %~d0%~p0..\..\..\config.bat
@if ERRORLEVEL 1 goto Error

REM  NOTE that this test does not do anything.
REM  PEVERIFY not needed

:Ok
endlocal
exit /b 0

:Error
call %SCRIPT_ROOT%\ChompErr.bat  %ERRORLEVEL% %~f0
endlocal
exit /b %ERRORLEVEL%
