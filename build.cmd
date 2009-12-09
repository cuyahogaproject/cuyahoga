@echo off

IF "%~dp0"=="%CD%\" goto checkargs
goto normal

:checkargs
IF "%~1"=="" goto quick

:normal
"%~dp0tools\NAnt\bin\NAnt.exe" -t:net-3.5 -D:nunit-console="%~dp0tools\NUnit\bin\nunit-console.exe" %*
goto end

:quick
"%~dp0tools\NAnt\bin\NAnt.exe" -t:net-3.5 -D:nunit-console="%~dp0tools\NUnit\bin\nunit-console.exe" quick release clean build
goto end

:test
ECHO test!

:end

