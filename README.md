# Overview

This is a mod for [Caves of Qud](https://www.cavesofqud.com) that allows unlocking basic skills through practical training, rather than spending skill points.

The main goal of this mod is to add a little bit of realism and some benefit to engaging with aspects of the game that aren't part of your build trajectory.

# Features

* In-game actions that train certain skills increase a special training point value every time it is performed (see below for more details). When the training point value reaches or exceeds the value of the corresponding skill, that skill is automatically unlocked.
* Most training targets "base" skills that unlock a whole skill tree (and in most cases also the first skill in the tree, if it has 0 cost).
* Skill points and training points are completely independent of each other. Training a skill does not make it cheaper to purchase with points, nor do any spent or unspent skill points affect the practical training.
* Each of the following categories has its own rate of training (how quickly the skill can be unlocked), configurable in mod's options.


## Wish command

Wish for `SkillTraining` to display the mod's main menu, where you can:
* See and reset training point progress for each trainable skill.
* Unlearn known skills, with or without refunding the skill points.


## Melee weapons

Using melee weapons in combat can train their corresponding base skills ([Axe](https://wiki.cavesofqud.com/wiki/Axe), [Cudgel](https://wiki.cavesofqud.com/wiki/Cudgel), [Short Blade](https://wiki.cavesofqud.com/wiki/Short_Blade) or [Long Blade](https://wiki.cavesofqud.com/wiki/Long_Blade)), as well as single or multi weapon fighting.

A melee weapon skill training increases every time you attack an enemy, if all the following are true:
* The target is successfully hit (with or without damage).
* The target is a creature (destroying walls or trees is not combat).
* The weapon is an equipped item (no natural weapons like fists or claws).
* The weapon is in the main hand (off-hand weapons don't train weapon skills).
* The attacking weapon has an associated skill to train (whips will not train anything).

Single/multi weapon fighting:
* If the attacking weapon is the only one equipped, [Single Weapon Fighting](https://wiki.cavesofqud.com/wiki/Single_Weapon_Fighting) will also be trained, at **half** the rate of the weapon skill.
* If the attacking weapon is equipped in an off-hand, it won't train its weapon skill, but will train [Multiweapon Fighting](https://wiki.cavesofqud.com/wiki/Multiweapon_Fighting) at **double** the rate of the weapon skill training.

## Missile weapons

Successfully hitting an enemy in combat trains the associated missile weapon skill ([Bow and Rifle](https://wiki.cavesofqud.com/wiki/Bow_and_Rifle), [Heavy Weapon](https://wiki.cavesofqud.com/wiki/Heavy_Weapon), or [Pistol](https://wiki.cavesofqud.com/wiki/Pistol)).

A missile weapon skill training increases every time you attack an enemy, if all the following are true:
* The target is successfully hit (with or without damage).
* The target is a creature (shooting walls and trees doesn't count).
* The target is the one originally aimed at (hitting something else by accident doesn't count).


## Thrown weapons

Successfully hitting an enemy in combat trains [Deft Throwing](https://wiki.cavesofqud.com/wiki/Deft_Throwing) skill. Note that de is an individual skill (a "power"), rather than a skill tree -- unlocking it will not automatically make the rest of the [Tactics](https://wiki.cavesofqud.com/wiki/Tactics) tree available.

Training increases every time you throw something at an enemy, if all the following are true:
* The target is successfully hit (with or without damage).
* The target is a creature (throwing at walls and trees doesn't count).
* The target is the one originally aimed at (hitting something else by accident doesn't count).


## Cooking

Cooking meal trains the [Cooking and Gathering](https://wiki.cavesofqud.com/wiki/Cooking_and_Gathering) skill. 