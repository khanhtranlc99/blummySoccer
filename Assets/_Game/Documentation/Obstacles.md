# Game Obstacles Documentation

## Overview
This document describes all obstacles available in the game, located in the `Assets/_Game/Prefabs/Gameplay/Obstacles` folder.

## Obstacle Types

### 1. Basic Obstacles
- **Box**
  - Static obstacle used for terrain building
  - Stationary object that blocks ball movement
  - No bounce effect when ball hits
  - Used for creating level geometry

- **BiggerBox**
  - Larger version of Box
  - Stationary obstacle for terrain building
  - Blocks ball movement without bounce effect
  - Used for creating larger level structures

- **Square**
  - Bouncy obstacle for terrain building
  - Creates bounce effect when ball hits
  - Used for creating dynamic level geometry
  - Ball will bounce off when hitting this obstacle

- **StaticSquare**
  - Special obstacle that creates wind effect
  - Features:
    - Creates air current to deflect ball movement
    - Configurable push force (default: 10)
    - Has animation system for visual feedback
    - Uses BoxCollider2D for collision detection
  - Technical Details:
    - Uses custom script for wind effect
    - Has animator component for visual effects
    - Can be configured in Unity Inspector
    - Affects ball physics without direct collision
  - Usage:
    - Place strategically to create air currents
    - Adjust push force for desired effect
    - Can be used to create challenging ball paths
    - Works in combination with other obstacles

### 2. Enemy Obstacles
- **Enemy**
  - Stationary enemy obstacle
  - Used as a target for ball shooting
  - No movement behavior
  - Ball can hit its face

- **Do_BirdRider**
  - Flying obstacle in the air
  - Creates aerial challenges
  - Used for creating vertical obstacles
  - Ball can pass underneath

### 3. Special Obstacles
- **Slime**
  - Moving obstacle with horizontal movement
  - Moves back and forth between walls
  - Automatically turns around when hitting walls
  - Creates dynamic challenges

- **SpiningPile**
  - Rotating obstacle
  - Spins 360 degrees continuously
  - Used to block ball movement
  - Creates dynamic barrier

- **Ice**
  - Slippery platform obstacle
  - Special physics properties:
    - Players slide when standing on it
    - Ball slides faster on ice surface
  - Used for creating slippery surfaces
  - Creates unique gameplay mechanics

- **BigBall**
  - Large bouncing ball obstacle
  - Dynamic movement with random bounces
  - Creates unpredictable obstacles
  - Used for dynamic level challenges

### 4. Spider Obstacles (Unused in Level Design)
- **Nhen**
  - Basic spider obstacle
  - Not used in level design
  - Reserved for special events or future use

- **Nhen_Do**
  - Red variant of the spider
  - Not used in level design
  - Reserved for special events or future use

## Technical Details
- All obstacles are implemented as prefabs
- Each obstacle has its own collision and physics properties
- Obstacles can be dynamically spawned during gameplay
- Some obstacles have special movement patterns and behaviors

## Usage Notes
- Obstacles are used to create challenging gameplay scenarios
- Different obstacles can be combined to create various difficulty levels
- Some obstacles may have special effects or animations
- Obstacle properties can be modified in the Unity Inspector

## Best Practices
1. Always test obstacle interactions thoroughly
2. Ensure proper collision layers are set
3. Optimize physics properties for smooth gameplay
4. Consider performance impact when spawning multiple obstacles
5. Use appropriate obstacles for intended gameplay mechanics
6. Test ball physics with different obstacle combinations

## Future Considerations
- Potential for new obstacle types
- Enhanced obstacle behaviors
- Special obstacle combinations
- Dynamic difficulty scaling based on obstacle placement 