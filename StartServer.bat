REM Remove the resources folder in case it lost the symlink, then recreate it
rmdir server\resources
mklink /j server\resources resources
cd server

set /p config="Enter name of settings config to use {prod|test|dev} Default: [dev]: "

IF "%config%"=="" (set file=settings.dev.xml) ELSE (set file=settings.%config%.xml)
del settings.xml
ECHO F|xcopy /y /v %file% settings.xml
call GTANetworkServer.exe