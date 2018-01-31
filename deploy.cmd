@if "%SCM_TRACE_LEVEL%" NEQ "4" @echo off

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Setup
:: -----
setlocal enabledelayedexpansion

IF NOT DEFINED DEPLOYMENT_SOURCE (
  SET DEPLOYMENT_SOURCE=%ARTIFACTS%\repository
)

IF NOT DEFINED DEPLOYMENT_TARGET (
  SET DEPLOYMENT_TARGET=%ARTIFACTS%\wwwroot
)

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Deployment
:: ----------
echo Source: %DEPLOYMENT_SOURCE%...
echo Deployment: in %DEPLOYMENT_TARGET%...

echo Server off
touch %DEPLOYMENT_TARGET%\App_Offline.htm

echo Deploy API
call :ExecuteCmd dotnet publish src\server\AspNetCore-SignalR.Api\ -o %DEPLOYMENT_TARGET%
IF !ERRORLEVEL! NEQ 0 goto error

pushd "%DEPLOYMENT_SOURCE%\src\client"
echo Npm install
call :ExecuteCmd npm install --production
IF !ERRORLEVEL! NEQ 0 goto error

echo Deploy client
call node_modules/.bin/ng build --env=prod --prod --output-path=%DEPLOYMENT_TARGET%
IF !ERRORLEVEL! NEQ 0 goto error
popd

echo Server on
rm %DEPLOYMENT_TARGET%\App_Offline.htm


::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: End
:: ---
IF !ERRORLEVEL! NEQ 0 goto error

goto end

:: Execute command routine that will echo out when error
:ExecuteCmd
setlocal
set _CMD_=%*
call %_CMD_%
if "%ERRORLEVEL%" NEQ "0" echo Failed exitCode=%ERRORLEVEL%, command=%_CMD_%
exit /b %ERRORLEVEL%

:error
endlocal
echo An error has occurred during web site deployment.
call :exitSetErrorLevel
call :exitFromFunction 2>nul

:exitSetErrorLevel
exit /b 1

:exitFromFunction
()

:end
endlocal
echo Finished successfully.
