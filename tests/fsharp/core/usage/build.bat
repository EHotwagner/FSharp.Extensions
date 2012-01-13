if "%_echo%"=="" echo off

setlocal
REM Configure the sample, i.e. where to find the F# compiler and C# compiler.
if EXIST build.ok DEL /f /q build.ok

call %~d0%~p0..\..\..\config.bat
@if ERRORLEVEL 1 goto Error

if NOT "%FSC:NOTAVAIL=X%" == "%FSC%" ( 
  ECHO Skipping test for FSI.EXE
  goto Skip
)

rem recall  >fred.txt 2>&1 merges stderr into the stdout redirect
rem however 2>&1  >fred.txt did not seem to do it.

echo == FSC usage
"%FSC%" %fsc_flags% --help > z.output.fsc.help.txt 2>&1
echo == FSI usage
"%FSI%" %fsc_flags% --help > z.output.fsi.help.txt 2>&1

if NOT EXIST z.output.fsc.help.bsl COPY z.output.fsc.help.txt z.output.fsc.help.bsl
if NOT EXIST z.output.fsi.help.bsl COPY z.output.fsi.help.txt z.output.fsi.help.bsl

%FSDIFF% z.output.fsc.help.txt z.output.fsc.help.bsl > z.output.fsc.help.diff
%FSDIFF% z.output.fsi.help.txt z.output.fsi.help.bsl > z.output.fsi.help.diff

echo ======== Differences From ========
TYPE  z.output.fsc.help.diff
TYPE  z.output.fsi.help.diff
echo ========= Differences To =========

TYPE  z.output.fsc.help.diff     > zz.alldiffs
TYPE  z.output.fsi.help.diff     >> zz.alldiffs

for /f %%c IN (zz.alldiffs) do (
  echo NOTE -------------------------------------
  echo NOTE ---------- THERE ARE DIFFs ----------
  echo NOTE -------------------------------------
  echo .  
  echo To update baselines: "sd edit *bsl", "del *bsl", "build.bat" regenerates bsl, "sd diff ...", check what changed.  
  goto Error
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
rem Hardwire ERRORLEVEL to be 1, since routes in here from diff check do not have ERRORLEVEL set
rem call %SCRIPT_ROOT%\ChompErr.bat %ERRORLEVEL% %~f0
call %SCRIPT_ROOT%\ChompErr.bat 1 %~f0
endlocal
exit /b %ERRORLEVEL%
