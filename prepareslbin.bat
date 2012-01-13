@echo off

if "%1"=="" goto :error

Rem sign the assemblies
sn -R %1\Silverlight\4.0\bin\FSharp.Core.dll src\fsharp\fsSilverlight.snk
sn -R %1\Silverlight\4.0\bin\FSharp.Hostable.dll src\fsharp\fsSilverlight.snk
sn -R %1\Silverlight\4.0\bin\Fsi.Hosted.dll src\fsharp\fsSilverlight.snk

Rem copy relevant assemblies to SL Bin folder
if not exist SLBin mkdir SLBin
copy /Y %1\Silverlight\4.0\bin\FSharp.Core.dll SLBin
copy /Y %1\Silverlight\4.0\bin\FSharp.Core.optdata SLBin
copy /Y %1\Silverlight\4.0\bin\FSharp.Core.sigdata SLBin
copy /Y %1\Silverlight\4.0\bin\FSharp.Core.xml SLBin
copy /Y %1\Silverlight\4.0\bin\FSharp.Core.pdb SLBin

copy /Y %1\Silverlight\4.0\bin\FSharp.Hostable.dll SLBin
copy /Y %1\Silverlight\4.0\bin\FSharp.Hostable.optdata SLBin
copy /Y %1\Silverlight\4.0\bin\FSharp.Hostable.sigdata SLBin
copy /Y %1\Silverlight\4.0\bin\FSharp.Hostable.xml SLBin
copy /Y %1\Silverlight\4.0\bin\FSharp.Hostable.pdb SLBin

copy /Y %1\Silverlight\4.0\bin\Fsi.Hosted.dll SLBin
copy /Y %1\Silverlight\4.0\bin\Fsi.Hosted.xml SLBin
copy /Y %1\Silverlight\4.0\bin\Fsi.Hosted.pdb SLBin

goto :end

:error
echo Please pass in a configuration as a parameter (E.g., prepareslbin.bat Release)

:end