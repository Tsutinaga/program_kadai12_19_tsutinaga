# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a Unity 6000.0.41f1 project implementing a 3D wave-based survival shooter game where the player faces waves of enemies with an auto-targeting weapon system.

## Unity Development Commands

### Opening the Project
- Open this project folder in Unity Hub with Unity 6000.0.41f1
- The main scene is located at: `Assets/Scenes/SampleScene.unity`

### Testing
- Run tests via Unity Test Runner: Window → General → Test Runner
- Play mode testing: Enter Play Mode in Unity Editor (Ctrl/Cmd + P)

### Building
- Build via File → Build Settings
- Supported platforms configured in ProjectSettings/EditorBuildSettings.asset

## Code Architecture

### Singleton Manager Pattern
The project uses singleton managers for global state management:
- **GameManager** (`Assets/scripts/GameManager.cs`): Central game state controller
  - Manages game over/clear conditions
  - Pauses game by setting `Time.timeScale = 0f`
  - Singleton accessible via `GameManager.Instance`
- **UIManager** (`Assets/scripts/UiManager.cs`): UI state management
  - Handles HP display, wave counter, and game end screens
  - Singleton accessible via `UIManager.Instance`

### Core Game Systems

#### Player System
- **PlayerController** (`Assets/scripts/playercontroller.cs`):
  - Movement via Input.GetAxis (Horizontal/Vertical)
  - HP management with damage flash effect
  - Collision-based damage from enemies (10 damage per collision)
  - Triggers GameOver when HP ≤ 0

#### Weapon System
- **WeaponSystem** (`Assets/scripts/WeaponSystems.cs`):
  - Auto-fires at nearest enemy based on fireRate
  - Uses GameObject.FindGameObjectsWithTag("Enemy") to find targets
  - Instantiates bullets with direction and speed
- **Bullet** (`Assets/scripts/Bullet.cs`):
  - Linear movement with 5-second lifetime
  - Trigger-based collision with enemies (25 damage)
  - Self-destructs on impact

#### Enemy System
- **Enemy** (`Assets/scripts/Enemy.cs`):
  - Chases player using normalized direction vector
  - HP-based health with damage flash feedback
  - Smooth shrink animation on death via coroutine
  - Uses Player tag to find target
- **EnemySpawner** (`Assets/scripts/EnemySpawner.cs`):
  - Wave-based spawning (configurable enemies per wave and total waves)
  - Spawns enemies in random positions within spawnRadius
  - Automatically progresses to next wave when all enemies defeated
  - Triggers GameClear after final wave

#### Camera System
- **CameraFollow** (`Assets/scripts/CameraFollow.cs`):
  - Smooth camera following with configurable offset
  - Fixed rotation at 45° angles for isometric view
  - Uses LateUpdate for smooth follow

### Unity Packages
Key dependencies (see `Packages/manifest.json`):
- `com.unity.inputsystem` (1.13.1): New Input System for player controls
- `com.unity.render-pipelines.universal` (17.0.4): URP for rendering
- `com.unity.ugui` (2.0.0): UI system
- TextMesh Pro: Text rendering (included assets in `Assets/TextMesh Pro/`)

### Prefabs
- `Assets/Prefabs/Enemy.prefab`: Enemy game object
- `Assets/Prefabs/Bullet.prefab`: Bullet projectile

### Tags Required
The game relies on Unity tags:
- `Player`: For player identification
- `Enemy`: For enemy identification and targeting

### Scene Structure
The main scene (`Assets/Scenes/SampleScene.unity`) should contain:
- Player object with PlayerController and WeaponSystem
- GameManager object with GameManager component
- UIManager object with UIManager component and UI references
- EnemySpawner object with EnemySpawner component
- Camera with CameraFollow component

## Code Conventions

### Naming
- C# scripts use PascalCase for class names (e.g., `GameManager`)
- One exception: `playercontroller.cs` (lowercase filename, but class is `PlayerController`)
- Public fields exposed in Inspector use camelCase with descriptive names

### Design Patterns
- Singleton pattern for managers with null-check in Awake()
- Component-based architecture following Unity conventions
- Coroutines for animations (e.g., enemy death shrink effect)
- Material color manipulation for visual feedback (damage flashes)

### Communication Between Systems
- Managers communicate via singleton pattern: `ManagerName.Instance?.MethodCall()`
- Null-conditional operator (`?.`) used to prevent null reference errors
- GameObject.FindGameObjectsWithTag() for runtime object queries
- GetComponent<>() for component access
