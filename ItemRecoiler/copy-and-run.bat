cd %~dp0
taskkill /f /im CoQ.exe
call copy-only.bat
start "" "c:\games\steam\steamapps\common\Caves of Qud\CoQ.exe"