call testList.bat
del failures.log
:repeat
for %%f in (%TESTS%) do call runOne.bat %%f
if '%1' == 'repeat' goto repeat
