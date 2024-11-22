cd %~dp0
taskkill /f /im CoQ.exe 
robocopy . "c:\users\martin\AppData\LocalLow\Freehold Games\CavesOfQud\Mods\SkillTraining" *.cs *.xml /s /purge
start "" "c:\games\steam\steamapps\common\Caves of Qud\CoQ.exe"