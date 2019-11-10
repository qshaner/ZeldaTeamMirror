# CSE 3902  
# Team: RiotSquad  
## Members: Chase Colman, Jarred Fink, Steven Neveadomi, Quinn Shaner, Henry Xiong  
  
## GOAL: To Implement the First Dungeon of the Original Legend of Zelda NES Game  
  
To this end, we are using Visual Studio and Monogame to recreate the dungeon using C#.   
  
# Technical Details  
  
## Current Design  
  
Currently, for keyboard controls we are using the Command Design Pattern.  
We are then using a State Machine for the individual enemy groups, in addition to the player.   
  
We have broken up our implementation by __nouns__, such as 'enemies', 'items', 'projectiles'. This was done to seperate the concerns of each of these sprite groups, and to break up the work into distinct pieces.  
  
__NEW:__
- As a team, we decided to use the command pattern for all collision effects
- After talking to Dr. Boggus and Ian, we determined that projectiles, while implemented, did not need to have collisions in sprint 3.
- The Enemies have a basic AI implemented
- Used the game mapping program Tiled to create our maps and export them as CSVs
- Large classes were broken into smaller, more manageable classes.
- Implemented basic design for Doors, and Barriers
- Created every room and a few debug rooms

## Sprint 2 Details  
  
Chase created base interfaces for the rest of us to use-- IMoveable, ISpawnable, IEnemy, ISprite, IPlayer, and INonPlayerCharacter. Chase has also implemented the player class, and refactored the sprite implementation.  
  
Jarred created texture atlases for all sprites, and resolved any major conflicts after everything was implemented. Jarred also learned how to use Git.  
  
Steven implemented NPC classes, Enemy classes and refactored Controllers. Both he and Chase have been leading discussions on design implementations.  
  
Quinn created the Readme, implemented item classes and implemented projectiles.   
  
Henry implemented the Block classes, and implemented the keyboard input and commands. Henry has also provided support for Chase.  
  

## Sprint 3 Details

Chase did a majority of the initial code reviews. He also created the initial design pattern and blueprint for all of us to work off of. It was invaluable to getting the project done. Chase created tilesets, the Dungeon Manager, Scene, and CSV asset pipeline library for MonoGame. In addition, Chase fixed a lot of bugs that came up and integrated the scene controller with everything else. Also designed the collision loop system and implemented item collision handling.

Jarred noticed a couple of visual bugs at the start of the sprint ⁠— items weren't aligned to the center of the box grid, and we were missing a few dungeon tiles. Jarred added an inventory system to Link to manage items. Jarred also worked on the projectile collisions and the behavior for the boomerang when thrown by the player and the Goriya. Jarred discovered a few other minor bugs and provided solutions that could be implemented.

Steven worked mainly on Enemies this sprint. He implemented the death and spawn effects, a basic enemy AI, created an agent for the Stalfos enemy, and ensured that both the player and the enemies could collide and interact with the current room. Steven also designed the Agent pattern. Both Steven and Chase determined how the scene would interact with both the player and enemies.

Quinn designed the command pattern for use with the Collideables, implemented Player Death and health checks, and implemented all item collisions. Quinn also created classes for all of the barriers and helped debug when needed. Quinn split up the sprint into main tasks and created most of the initial cards. Quinn did a lot of regression testing and double checking to ensure they were using the correct patterns.

Henry created classes for doors and stairs, the Room Loader, and the Jump Mini-Map Screen. The Jump Mini-Map Screen was a major component of this sprint, and required a lot of moving parts to be implemented before Henry could work on it. Henry also reviewed a lot of the pull requests as they happened. 

## Code Reviews  
  
The team, as a whole, has decided that Code Reviews will be kept in a central branch on each sprint. This central branch will get merged at the end of every sprint.  
Every major code review will be a file, consisting of both a review for readability and maintainability. The code review file will be broken down by file.  
  
~~__Major Code Reviews__ take place on a pull request. This code review will be labeled like so:~~  
  
~~PR#-NameOfPRBranch~~   
  
~~Every code review file will have every file that was in the particular PR listed and detailed, file by file.~~  

__NEW:__ For this sprint, most of our code reviews were done directly on PRs. Instead of doing Major Code Reviews on each PR, each team member did an in-depth review of a single file. These reviews are in the Sprint3 folder.
  
In addition, every Sprint will have it's own subfolder in the CodeReview folder.   
  
Further, the team has agreed that all members of the team should attempt to look at the PR, and one member will be assigned the in depth code review. As a member is looking at the PR, they are able to comment on specific lines of code for clarification, changes, or suggestions. These comments are kept in the PR, and typically short enough that they are not included in the Major Code Review file for that PR unless a change was made.  

## Code Analysis

Code Analysis results before and after fixes for each sprint can be found under the Code Analysis folder.
  
----
# General Information  
### Known bugs, controls, extra processes  
  
## Controls  

__Q__: Quit
__R__: Reset

__Z__: Primary Attack (Sword)  
__2__: Upgrade to White Sword
__3__: Upgrade to Magical Sword

__X__: Secondary Attack
__4__: Assign and Use the Bow as the Secondary Weapon  
__5__: Assign and Use the Boomerang as the Secondary Weapon  
__6__: Assign and Use the Bomb as the Secondary Weapon  

__W/UP__: Move Link Up  
__A/LEFT__: Move Link Left  
__S/DOWN__: Move Link Down  
__D/RIGHT__:  Move Link Right  

__SPACE__: Pause the game and bring up the inventory screen
__M__: Open up the map. _Click on each room to teleport to the room_  
  
## Frame Rates  
__Normal Frame Rate__: Frame rate usually used to cycle between animation frames of a sprite  
__Hurt Frame Rate__: Appears to be twice as fast as the normal frame rate, and used to swap palettes  
  
Individual sprites are aligned at multiples of 8, and most are aligned at multiples of 16.   
Dimensions of sprites are also in multiples of 8 or 16.   
  
Boss (48 X 256), individual frame: 24 X 32  
Doors (160 X 128), individual frame: 16 X 16  
Enemy Goriya (32 X 256), individual frame: 16 X 16  
Enemy Hand (32 X 48), individual frame: 16 X 16  
Enemy Other (32 X 48), individual frame: 16 X 16  
Enemy Skeleton (32 X 64), individual frame: 16 X 16  
Field Weapons (64 X 136), individual frame: 16 X 16, rows 1-4 swords, 7-8 fireballs  
	Boomerang (64 X 136), individual frame: 8 X 8, row 9  
Items (32 X 192), individual frame: 8 X 16 or 16 X 16  
Red/Blue Hearts (32 X 192), individual frame: 8 X 8  
Link No Weapon (32 X 288), individual frame: 16 X 16  
Link Sword (128 X 384), Link facing up/down: 16 X 32, Link facing left/right: 32 X 16  
Link Use Secondary (64 X 64), individual frame: 16 X 16  
Old Man (16 X 64), individual frame: 16 X 16  
Particles (64 X 120), individual frame: 16 X 16  
Tiles (32 X 80), individual frame: 16 X 16  
  
__Note on Sprite Animation__: The Sprite method takes in the Spritesheet, the width and height of the specific sprite, the frame count for animations, the offset from the spritesheet, and optionally takes in a frame delay, palette height, palette count, and palette shift delay.   
  Palette shifts most often occur when an entity is damaged, and thus not a required parameter for most sprites. 

## Enemies  
In regard to current design, each monster has its own class which then has its own agent class.   
The agent class is roughly an expanded state machine that includes the drawing logic based on the state.  
Stalfos and Goriya are meant to be knocked back when they take damage from Link, but they currently do not. This will be amended in Sprint 4.
   
The following are all the monsters currently implemented with their behavior explained:  
 - __Stalfos__: The skeleton. Takes 2 hits and dies.
 - __Keese__: The bat. Dies instantly.
 - __Wall Master__: The hand. Takes 2 hits and dies.
 - __Goriya__: The goblin. Takes 3 hits and dies. Can throw a boomerang.
 - __Trap__: The blue cross. Has no health and can't be damaged.
 - __Aquamentus__: The dragon. Faces only one direction in the game. Takes six hits and dies.
 - __Gel__: The gel drop. Dies instantly.
 - __Old Man__: The old man. Takes damage but is immortal.

## Doors
As we have yet to implement Link's ability to walk between rooms, the ability to walk through doors has not yet been implemented.  
For the time being, if Link collides with a door, he will simply be knocked back.  

If you need to jump between room use the jump map using the M key. You can click the text at the top to close the jump map.
  
## Bugs
Please check out our [bug report](bugs.md)
  
## Sprite Resources  
  
- __Link__: https://www.spriters-resource.com/nes/legendofzelda/sheet/8366/  
- __Dungeon Enemies__:  https://www.spriters-resource.com/nes/legendofzelda/sheet/31806/  
- __Items and Weapons__: https://www.spriters-resource.com/nes/legendofzelda/sheet/54720/  
- __NPCs__: https://www.spriters-resource.com/nes/legendofzelda/sheet/21189/  
- __Bosses__: https://www.spriters-resource.com/nes/legendofzelda/sheet/36632/  
- __Dungeon Tileset__: https://www.spriters-resource.com/nes/legendofzelda/sheet/8376/  
- __HUD & Pause Screen__: https://www.spriters-resource.com/nes/legendofzelda/sheet/119278/  


## Sound Resources
- __Background Music__: https://downloads.khinsider.com/game-soundtracks/album/the-legend-of-zelda-nes
- __Sprite Sound Effects: http://noproblo.dayjo.org/ZeldaSounds/LOZ/index.html
  
## Debug Rooms
There are four debug rooms, each with their own element of the game to test.
- __3-5__: Enemies
- __4-0__: Items
- __4-5__: Movable Blocks
- __5-5__: Locked Doors

    
## Extra Processes  
  
- Our method for pull requests and reviews have been expanded from the default of the class.   
- We are using an expanded form of the State Machine.  
- We are using Git, and all of the tools associated with Git.   
- All spritesheets have been configured to be texture atlasses.   
- We are meeting outside of class at least 2 times a week, for around an hour per meeting.  
- We have included more items and enemies than the default. 

__NEW:__
- We have augmented the state machines further with agents
- We are using the command pattern for collision effects
- We have a Dungeon Room loader, Manager, and Scene to implement the game world
- We broke down the Sprint into goals, then tasks, and then assigned point values to each task and ensured that the points were roughly evenly distributed  
- Interfaces were broken down by usage rather than by type ⁠— allowing shared utility to be implemented in separate concrete classes.  

