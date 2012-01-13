@if "%_echo%"=="" echo off

set _SCRIPT_DRIVE=%~d0
set _SCRIPT_PATH=%~p0
set SCRIPT_ROOT=%_SCRIPT_DRIVE%%_SCRIPT_PATH%

if exist "%SCRIPT_ROOT%..\..\Debug\cli\2.0\bin\FSharp.Core.dll" (
  echo Installing Debug FSharp.Core.dll for .NET 2.0
  gacutil /i %SCRIPT_ROOT%..\..\Debug\cli\2.0\bin\FSharp.Core.dll

)
if exist "%SCRIPT_ROOT%..\..\Debug\cli\4.0\bin\FSharp.Core.dll" (
  echo Installing Debug FSharp.Core.dll for .NET 4.0
  gacutil /i %SCRIPT_ROOT%..\..\Debug\cli\4.0\bin\FSharp.Core.dll

)
if exist "%SCRIPT_ROOT%..\..\Debug\cli\2.0\bin\fsc.exe" (
  echo Installing Debug FSharp.Core.dll for .NET 2.0
  ngen install %SCRIPT_ROOT%..\..\Debug\cli\2.0\bin\fsc.exe
)
if exist "%SCRIPT_ROOT%..\..\Debug\cli\4.0\bin\fsc.exe" (
  echo Installing Debug FSharp.Core.dll for .NET 4.0
  ngen install %SCRIPT_ROOT%..\..\Debug\cli\4.0\bin\fsc.exe
)
