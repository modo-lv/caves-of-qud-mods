cd %~dp0
robocopy .       "c:\users\martin\AppData\LocalLow\Freehold Games\CavesOfQud\Mods\ParasangRegion"      *.cs *.xml *.png *.json /s /purge /xd bin /xd obj
robocopy ..\Core "c:\users\martin\AppData\LocalLow\Freehold Games\CavesOfQud\Mods\ParasangRegion\Core" *.cs *.xml /s /purge /xd bin /xd obj
