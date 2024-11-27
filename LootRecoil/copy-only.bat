cd %~dp0
robocopy .       "c:\users\martin\AppData\LocalLow\Freehold Games\CavesOfQud\Mods\LootRecoil"      *.cs *.xml /s /purge /xd bin /xd obj
robocopy ..\Core "c:\users\martin\AppData\LocalLow\Freehold Games\CavesOfQud\Mods\LootRecoil\Core" *.cs *.xml /s /purge /xd bin /xd obj
