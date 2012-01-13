@if "%_echo%"=="" echo off

set _SCRIPT_DRIVE=%~d0
set _SCRIPT_PATH=%~p0
set _SCRIPT_ROOT=%_SCRIPT_DRIVE%%_SCRIPT_PATH%

set PATH=%ProgramFiles%\Microsoft SDKs\Windows\v7.0A\bin\NETFX 4.0 Tools;%ProgramFiles%\Microsoft Visual Studio 10.0\Common7\IDE;%_SCRIPT_ROOT%\Release\cli\4.0\bin;%_SCRIPT_ROOT%\Proto\built\cli\4.0\bin;%SystemRoot%\Microsoft.NET\Framework\v4.0.30319;%PATH%

set FSHARP_HOME=%_SCRIPT_ROOT%

set TARGETFSHARP=VS2010

color 6
title "%_SCRIPT_ROOT% Test Environment"
