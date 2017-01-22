REM Remove the resources folder in case it lost the symlink, then recreate it
rmdir server\resources
mklink /j server\resources resources
cd server
call GTANetworkServer.exe