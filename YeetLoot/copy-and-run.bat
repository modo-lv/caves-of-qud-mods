cd %~dp0
taskkill /f /im CoQ.exe
robocopy ..\Core "c:\users\martin\AppData\LocalLow\Freehold Games\CavesOfQud\Mods\YeetLoot" *.cs *.xml /s /purge /xd bin /xd obj
robocopy . "c:\users\martin\AppData\LocalLow\Freehold Games\CavesOfQud\Mods\YeetLoot" *.cs *.xml /s /xd bin /xd obj
start "" "c:\games\steam\steamapps\common\Caves of Qud\CoQ.exe"