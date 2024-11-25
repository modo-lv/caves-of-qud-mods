cd %~dp0
taskkill /f /im CoQ.exe
robocopy ..\Core "c:\users\martin\AppData\LocalLow\Freehold Games\CavesOfQud\Mods\SkillTraining" *.cs *.xml /s /purge /xd bin
robocopy . "c:\users\martin\AppData\LocalLow\Freehold Games\CavesOfQud\Mods\SkillTraining" *.cs *.xml /s /purge /xd bin
start "" "c:\games\steam\steamapps\common\Caves of Qud\CoQ.exe"