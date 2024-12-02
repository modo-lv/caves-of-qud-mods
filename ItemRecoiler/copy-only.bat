cd %~dp0
robocopy .       "c:\users\martin\AppData\LocalLow\Freehold Games\CavesOfQud\Mods\ItemRecoiler"      *.cs *.xml workshop.json preview.png /s /purge /xd bin /xd obj
robocopy ..\Core "c:\users\martin\AppData\LocalLow\Freehold Games\CavesOfQud\Mods\ItemRecoiler\Core" *.cs *.xml /s /purge /xd bin /xd obj
