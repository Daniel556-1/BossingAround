# Bossing Around
Team 13 - Gamers (Daniel & Kailer)
## What is Bossing Around?
Bossing Around is a first person action roguelike where the player is in a cyberpunk themed world. The player is tasked to load in as an agent to fight an evil robot boss. Every time the robot is defeated, the player can salvage its parts to get stronger, but be warned, the robot will also continue to get stronger to defeat the agent. The goal of this game is to make it through as many loops as possible without dying.
## Controls
WASD - Movement
Space - Jump
Left Click - Shoot
Left Shift - Dash (Dashes in the direction of movement)
## Inspirations
**Hyper Light Drifter** - We liked the pastel color schemes of bright colors.  
**Vampire Survivors** - We liked the gameplay loop of becoming stronger to fight ever-stronger monsters.  
**Returnal** - We liked how this game was a 3d bullet hell which is rare in the game industry (third person though).  
## Where can I play it?
You can either build it yourself **OR** we have it uploaded on itch.io at the link here:
> https://dawaterdrinker.itch.io/bossing-around
## File Structure
All of the relevant game files are located in the **Assets** directory. From there, here is a breakdown of all of the subdirectories.
#### Animations
This directory contains all animation files that are used in Bossing Around. It also includes the animation controllers that are used in bossing around. As of now, there is only an animation controller for the boss since it's the only thing that is visible and needs animations.
#### Audio
This directory contains two more subdirectories named **Music** and **SFX**. The music folder contains the music we use in our game. The SFX folder contains all sound effects including voice lines and UI feedback sounds.
#### Materials
This directory has all the materials we use in our game. The materials include stuff we use for 3d builds, UI, and also partical effects.
#### Prefabs
This directory is for prefabs. Some of them aren't actually used since we ran out of time to add new features, but the main ones are the boss prefab (used for instantiating the boss each loop), the player prefab (used for instantiating the player each loop) and the projectile prefabs like rocket, gladius, and flatrocket.
#### Scenes
This is where all the scenes are located. There are two main scenes, Main Menu and Game.
#### Scripts
This is where all the scripts are. These scripts control everything from the boss's AI to camera transitions in the UI, to the game music and volume, the player's movement system, etc. Please refer to the scripts themselves for more information. The most important scripts are the boss's AI and the state manager for the gamestate. For the state manager especially since the game relies on it for everything relating to the actual gameplay itself.
#### Settings
This is just a byproduct of using URP 3d. We did not edit this.
#### Sprites
This is where we store our sprites. Sprites were used for our UI
#### TextMeshPro
In this directory, we have all of our fonts that we used stored in there. There are also all of our font-variants that we used in there.
#### Textures
This is where all the textures are stored for the materials that we use for our 3d objects and 
