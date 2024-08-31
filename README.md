# UltraHub

## Overview

UltraHub is a 3D multiplayer game developed using Unity. The game allows players to join or create servers, customize their characters, and interact with other players in a dynamic and engaging environment.

## Functionality Design

When creating a 3D graphic application, it is crucial to define the tasks and goals clearly and to develop efficient algorithms. The following methods are used in the program:

- **Player Movement in Local Network:**
  - Players can move within the game environment, and their movements are synchronized with the game server.

- **Rendering and Synchronization:**
  - Players are rendered and synchronized on the game server to ensure smooth gameplay.

- **Aim Control with Mouse:**
  - Players can control their aim using the mouse.

- **Pseudo-Random Player Spawning:**
  - Players spawn at pseudo-random locations on the map and respawn after death.

- **Player Tracking:**
  - The current number of players and their characteristics are tracked.

### Pseudo-Random Player Spawning

An algorithm is used to spawn players at different locations on the map and to respawn them after death.

### Player Movement

Player movement is processed locally and then synchronized with the server.

### Player Interactions

Player interactions are processed locally and then synchronized with the server.

## User Manual

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

### Player Controls

- **W Key:** Moves the character forward on the map, animating the movement.
- **S Key:** Moves the character backward on the map, animating the movement.
- **A Key:** Moves the character to the left on the map, animating the movement.
- **D Key:** Moves the character to the right on the map, animating the movement.

### In-Game Information

In the lower-left corner, you will see information about player kills and deaths in the game world.
