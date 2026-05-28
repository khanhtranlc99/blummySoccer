# Level Components Documentation

## Overview
This document describes the main components used in level design, located in the `Assets/_Game/Prefabs/Gameplay` folder.

## Core Components

### 1. Player
- **Location**: `Player.prefab`
- **Purpose**: Main character controlled by the player
- **Features**:
  - Controllable movement
  - Ball interaction capabilities
  - Physics-based interactions
  - Customizable properties

### 2. Ball
- **Location**: `Ball.prefab`
- **Purpose**: Main gameplay object that players interact with
- **Features**:
  - Physics-based movement
  - Bounce and collision properties
  - Interaction with all level components
  - Core gameplay element

### 3. Goal
- **Location**: `Goal.prefab`
- **Purpose**: Target area for scoring points
- **Features**:
  - Ball detection
  - Score triggering
  - Level completion check
  - Visual feedback

### 4. Ground Components
- **Location**: `Ground/` folder
- **Purpose**: Level boundaries and platforms
- **Types**:
  - **Ground**: Basic ground platform
  - **Ground_2**: Alternative ground variant
  - **DecorWall**: Decorative wall elements
- **Features**:
  - Collision detection
  - Player and ball blocking
  - Level structure building
  - Visual environment creation

### 5. Limit System
- **Components**:
  - **TopLeft**: Upper boundary marker
  - **BottomRight**: Lower boundary marker
- **Purpose**: Camera boundary control
- **Features**:
  - Camera movement limits
  - Level boundary definition
  - Required in all maps
  - Viewport control

### 6. Environment System
- **Location**: `Environments/` folder
- **Purpose**: Level structure and layout
- **Components**:
  - Ground elements
  - Wall structures
  - Platform arrangements
- **Features**:
  - Level geometry creation
  - Obstacle placement
  - Gameplay area definition
  - Visual environment setup

## Level Structure

### Required Components
Every level must contain:
1. Player spawn point
2. Goal placement
3. Limit markers (TopLeft and BottomRight)
4. Ground boundaries
5. Environment setup

### Optional Components
1. Decorative elements
2. Special effects
3. Background elements
4. Additional gameplay mechanics

## Technical Details
- All components are implemented as prefabs
- Components use Unity's physics system
- Proper layer settings for collisions
- Optimized for performance
- Modular design for easy level creation

## Best Practices
1. Always include required components
2. Ensure proper component placement
3. Test all interactions thoroughly
4. Optimize level geometry
5. Maintain consistent spacing
6. Consider gameplay flow
7. Test camera boundaries
8. Verify collision settings

## Level Design Guidelines
1. Start with basic structure
2. Add gameplay elements
3. Place obstacles strategically
4. Test player movement
5. Verify ball physics
6. Check camera coverage
7. Add decorative elements
8. Final testing and optimization

## Future Considerations
- Enhanced component interactions
- New environment types
- Advanced physics features
- Dynamic level elements
- Improved visual effects
- Additional gameplay mechanics 