cd %~dp0
taskkill /f /im CoQ.exe
robocopy .        "c:\users\martin\AppData\LocalLow\Freehold Games\CavesOfQud\Mods\SkillTraining"     *.cs *.xml /s /purge /xd bin /xd obj
robocopy ..\Core "c:\users\martin\AppData\LocalLow\Freehold Games\CavesOfQud\Mods\SkillTraining\Core" *.cs *.xml /s /purge /xd bin /xd obj
start "" "c:\games\steam\steamapps\common\Caves of Qud\CoQ.exe"