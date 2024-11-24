# Overview

This is a mod for [Caves of Qud](https://www.cavesofqud.com) that allows unlocking basic skills through practical training, rather than spending skill points.

The main goal of this mod is to add a little bit of realism and some benefit to engaging with aspects of the game that aren't part of your build trajectory.

# Mod menu

Wish (`Ctrl+W` by default) for `SkillTraining` to display the mod's main menu, where you can:
* See and reset training point progress for each trainable skill.
* Unlearn known skills, with or without refunding the skill points.


# In detail

* In-game actions that train certain skills increase a special training point value every time it is performed (see below for more details). When the training point value reaches or exceeds the value of the corresponding skill, that skill is automatically unlocked.
* Most training targets "base" skills that unlock a whole skill tree (and in most cases also the first skill in the tree, if it has 0 cost).
* Skill points and training points are completely independent of each other. Training a skill does not make it cheaper to purchase with points, nor do any spent or unspent skill points affect the practical training.
* Each of the following categories has its own rate of training (how quickly the skill can be unlocked), configurable in mod's options.
* Skill unlocks only happen at the moment of training. For example, if you have fully trained a skill, but it hasn't unlocked because of an attribute requirement, you will need to train again after meeting the requirement to trigger the unlock.


## Customs and Folklore

[Customs and Folklore] is trained whenever you gain reputation from a water ritual. Note that unlocking it requires at least 19 Intelligence. It can still be trained, but will only unlock once the requirement is met. 

## Cooking

Cooking meal trains the [Cooking and Gathering](https://wiki.cavesofqud.com/wiki/Cooking_and_Gathering) skill. "Tasty" meals double the point increase.


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


## Physic

Successfully applying bandages to bleeding wounds trains [Physic](https://wiki.cavesofqud.com/wiki/Physic) skill.


## Shield

Blocking an attack with shield trains the [Shield](https://wiki.cavesofqud.com/wiki/Shield) skill.


## Swimming

Swimming in deep water automatically trains [Swimming](https://wiki.cavesofqud.com/wiki/Swimming) skill.

Note that this is an individual skill (a "power"), rather than a skill tree -- unlocking it will not automatically make the rest of the [Endurance](https://wiki.cavesofqud.com/wiki/Endurance) tree available.


## Snake Oiler

Every completed trade trains [Snake Oiler](https://wiki.cavesofqud.com/wiki/Snake_Oiler) 

Note that this is an individual skill (a "power"), rather than a skill tree -- unlocking it will not automatically make the rest of the [Persuasion](https://wiki.cavesofqud.com/wiki/Persuasion) tree available.


## Thrown weapons

Successfully hitting an enemy in combat trains [Deft Throwing](https://wiki.cavesofqud.com/wiki/Deft_Throwing) skill.

Note that this is an individual skill (a "power"), rather than a skill tree -- unlocking it will not automatically make the rest of the [Tactics](https://wiki.cavesofqud.com/wiki/Tactics) tree available.

Training increases every time you throw something at an enemy, if all the following are true:
* The target is successfully hit (with or without damage).
* The target is a creature (throwing at walls and trees doesn't count).
* The target is the one originally aimed at (hitting something else by accident doesn't count).


## Wayfaring

[Wayfaring](https://wiki.cavesofqud.com/wiki/Wayfaring) skill is trained whenever you take a step on the world map. The training is increased every 300 turns (<=1 step on the world map, depending on terrain), and regaining your bearings after getting lost adds training point value 5 times the normal rate. 