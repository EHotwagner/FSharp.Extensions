@if "%_echo%"=="" echo off

set _SCRIPT_DRIVE=%~d0
set _SCRIPT_PATH=%~p0
set _SCRIPT_ROOT=%_SCRIPT_DRIVE%%_SCRIPT_PATH%

set PATH=%ProgramFiles%\Microsoft SDKs\Windows\v6.0A\bin;%ProgramFiles%\Microsoft Visual Studio 9.0\Common7\IDE;%_SCRIPT_ROOT%\Debug\cli\2.0\bin;%_SCRIPT_ROOT%\Proto\built\cli\2.0\bin;%SystemRoot%\Microsoft.NET\Framework\v3.5;%SystemRoot%\Microsoft.NET\Framework\v2.0.50727;%PATH%

set FSHARP_HOME=%_SCRIPT_ROOT%

set TARGETFSHARP=VS2008

color 6
title "%_SCRIPT_ROOT% Test Environment"
