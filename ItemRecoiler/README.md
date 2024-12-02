# Overview

This is a mod for [Caves of Qud](https://www.cavesofqud.com) that adds a different kind of recoiler to the game -- one that recoils items, not the player. It mostly functions just like a regular [reprogrammable recoiler](https://wiki.cavesofqud.com/wiki/Reprogrammable_recoiler), except when activated, it displays a container window, and any items placed in it when the window closes are teleported to the imprinted location.

Most other aspects are unchanged -- it requires energy to function, it can be found, bought, sold, created from a schematic or disassembled for bits, etc.

# Features and details

* **Acquiring**: All characters automatically get one free recoiler (with a [solar cell](https://wiki.cavesofqud.com/wiki/Solar_cell)) per game at the start of the game, or when loading a game after installing the mod. This can be turned off in options.
* **Holographic receiver**: When you imprint an item recoiler, it places a holographic receiver (shaped like a purple-colored chest) as a marker in that location. This marks the place for recoil and makes item access more convenient, however, the box itself is a hologram and can't be interacted with (picked up, destroyed, etc.). And when the recoiler is programmed with a new location, the receiver will move, but any items still "in" it will remain.
* **Recoiling the recoiler**: Item recoilers themselves can't be recoiled. If placed in the teleportation "container", they will simply return to the player's inventory once the teleportation begins. 
* **Energy cost**: The energy cost of recoiling items is equal to their total weight, rounded up to the nearest full pound. If you're using a low energy cell (such as the starting solar one), beware of recoiling heavy objects (corpses, stones) unless you have a way to recharge.
* **Keyboard shortcut**: If you're recoiling items frequently, you can assign a keyboard shortcut for quick activation in the game's control mappings (I like `Shift+Q`). Note that quick activation can't choose between multiple item recoilers, you'll have to use the regular Recoil ability if you're carrying more than one.
* **Find imprint**: The recoiler changes its name depending on where it's imprinted, but if that's not enough to find a lost imprint point, you can use the "find imprint" function. It will tell you the world map coordinates (`X:Y`, starting from the top left corner), parasang name, zone (one of the 9 regions that each parasang has) and depth (if the parasang has more than the surface level).

# License and source code

This work © 2024 is licensed under [Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International](https://creativecommons.org/licenses/by-nc-sa/4.0/).

Source code is hosted on [GitHub](https://github.com/modo-lv/caves-of-qud-mods/tree/main/ItemRecoiler).