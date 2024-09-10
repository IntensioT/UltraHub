# UltraHub

## Overview

UltraHub is a 3D multiplayer game developed using Unity. The game allows players to join or create servers, customize their characters, and interact with other players in a dynamic and engaging environment.
The character must move from the starting point to the finish line, overcoming obstacles on the field.

### Field

- Several paths branch out from the starting point, intertwined with each other.
- All paths converge into one at the finish line.
- Some blocks on the path are traps that hinder the player from reaching the finish.

### Traps

1. **Damage Block**:
   - Activates when the player steps on it (glows orange).
   - Deals damage to everyone standing on the block after 1 second (flashes red).
   - Recharges over 5 seconds.

2. **Wind Block**:
   - Wind pushes the character with a certain force.
   - Only affects the character while they are on the block.
   - Wind direction changes randomly every 2 seconds.
   - Wind blows strictly horizontally.

### Game End

- **Victory**:
  - The player crosses the finish line.
  - A "Victory!" message and a restart button are displayed.
  - The level completion time is shown.

- **Defeat**:
  - The player falls off the path into the abyss or runs out of health.
  - A "Defeat!" message and a restart button are displayed.

### Additional

- The player's health is always displayed on the screen.
- The level completion time is calculated from the moment the starting line is crossed.
- In the lower-left corner, you will see information about player kills and deaths in the game world.

## Functionality Design

The following methods are used in the program:

- **Player Movement in Local Network:**
  - Players can move within the game environment, and their movements are synchronized with the game server.

- **Rendering and Synchronization:**
  - Players are rendered and synchronized on the game server to ensure smooth gameplay.

- **Aim Control with Mouse:**
  - Players can control their aim using the mouse.

- **Pseudo-Random Player Spawning:**
  - Players spawn at pseudo-random locations on the map and respawn after death.

## User Manual
### Player Controls

- **W Key:** Moves the character forward on the map, animating the movement.
- **S Key:** Moves the character backward on the map, animating the movement.
- **A Key:** Moves the character to the left on the map, animating the movement.
- **D Key:** Moves the character to the right on the map, animating the movement.
- **V Key:** Changes third/first person view.
- **Left mouse button** Fire button.
- **Right mouse button** Rocket fire button.
- **R key** Grenade throw button.
- **Space bar key** Jump and double-jump button.

### Controlling the Game

After launching the application, you will be greeted by the main menu:

![image](https://github.com/user-attachments/assets/3a45114b-6c19-4616-abed-f4ceeb4608fb)

1. **Input Field for Server Name:**
   - Enter your server name here.

![image](https://github.com/user-attachments/assets/3d4b05ba-bd80-4f04-a445-b3e2c93e5361)

2. **Server List Button:**
   - Opens a list of available servers. You can join an existing server or create your own.

![image](https://github.com/user-attachments/assets/c165a6ed-e0fd-45fc-9cff-171935321e6f)

After joining or creating a lobby, you can customize the appearance of your character.

![image](https://github.com/user-attachments/assets/4c5de0a6-d4bd-425f-a0c4-93b74d472fd6)

