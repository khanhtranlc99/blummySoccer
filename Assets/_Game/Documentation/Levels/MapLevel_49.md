# MapLevel_49 Documentation

## Overview
MapLevel_49 is a challenging level featuring various obstacles and platforms. The level uses theme 5 and includes multiple sections with different gameplay elements. The level is designed to test player's platforming skills and timing.

## Core Components

### 1. Root Object (MapLevel_49)
- **Position**: (0, 0, 0)
- **Scale**: (1, 1, 1)
- **Rotation**: (0, 0, 0)
- **Components**:
  - Transform
  - LevelManager (MonoBehaviour)
    - Script: 9ea9b98ab7b5b1d45b44891137c0f5b8
- **Properties**:
  - Theme: 5
  - Background: Custom background asset (Script: ca108e8da04f3ad4298c7de90992862b)
  - Camera Limits: Properly configured

### 2. Camera Boundaries
- **Limit Container**
  - Position: (1.79, 3.81, 0)
  - Scale: (1, 1, 1)
  - Rotation: (0, 0, 0)
  - Children:
    - TopLeft: (-6.95, 0.1, 0)
    - BottomRight: (5.56, -7.03, 0)
  - Purpose: Define camera movement boundaries

### 3. Player
- **Player**
  - Position: (-6.82, 7.1, 0)
  - Scale: (1, 1, 1)
  - Rotation: (0, 0, 0)
  - Components:
    - Transform
    - Player Controller
    - Rigidbody2D
      - Mass: 5 (Adjusted for better physics control)
    - Collider2D
  - Purpose: Main character controlled by the player
  - Prefab Reference: 1bb45bd9a0021eb46bfeb9c21f4e57b3

### 4. Environment
- **Environment Container**
  - Position: (-0.61, -8.23, 0)
  - Scale: (1, 1, 1)
  - Rotation: (0, 0, 0)
  - Contains:
    - Ground_3:
      - Position: (-6.87, 13.32, 0)
      - Size: (4.85, 1.90)
      - Components:
        - BoxCollider2D
        - SpriteRenderer
      - Purpose: Starting platform
    - Ground_2:
      - Position: (9.78, 6.19, 0)
      - Size: (4.37, 1.86)
      - Components:
        - BoxCollider2D
        - SpriteRenderer
      - Purpose: Main traversal platform
    - SpiningPile:
      - Position: (1.5, 3.26, 0)
      - Scale: (2, 2, 2)
      - Rotation: (0, 0, 0)
      - Properties:
        - Duration: 0.7
      - Prefab Reference: 6e435bffb5971cc48bc31f0d851f6848
      - Purpose: Rotating obstacle that adds challenge to navigation

### 5. Special Features
- **Do_BirdRider**
  - Position: (7.21, 3.86, 0)
  - Rotation: (0, 0, 0)
  - Scale: (1, 1, 1)
  - Prefab Reference: a75ab555c92d03742beb12742a0454e0
  - Purpose: Interactive element for player movement assistance

### 6. Goal
- **Goal Object**
  - Position: (9.81, 0.80, 0)
  - Rotation: (0, 180, 0)
  - Scale: (1, 1, 1)
  - Purpose: Level completion trigger
  - Prefab Reference: 5f48919df8d0fd344a136a3dcfe732c9

## Level Sections

### Starting Area
- **Initial Platform (Ground_3)**
  - Elevated position for level start
  - Wide platform for safe landing
  - Connected to main progression path

### Mid-Section
- **Main Platform (Ground_2)**
  - Strategic placement for progression
  - Requires precise jumping
  - Connects to special features
- **SpiningPile Obstacle**
  - Rotating hazard with 0.7s rotation duration
  - Requires timing to navigate
  - Positioned between platforms for added challenge

### Advanced Section
- **BirdRider Integration**
  - Positioned for optimal path
  - Requires timing and skill
  - Leads to goal area

## Technical Details

### Component References
- All prefabs properly referenced
- Scripts correctly assigned
- Colliders and triggers configured
- Physics properties tuned

### Physics Settings
- Player mass: 5 (optimized for control)
- Platform colliders sized appropriately
- Gravity settings balanced for gameplay
- Collision layers properly set

### Performance Considerations
- Minimal object count
- Optimized collider shapes
- Efficient trigger usage
- Background handling optimized

## Testing Points

### Core Functionality
1. Player spawn and movement
2. Platform collision accuracy
3. BirdRider interaction
4. Goal trigger response
5. Camera boundary behavior

### Performance Metrics
1. Frame rate stability
2. Physics performance
3. Memory usage
4. Load time optimization
5. Collision accuracy

### Gameplay Balance
1. Jump distances
2. Platform placement
3. Difficulty progression
4. BirdRider timing
5. Overall flow

## Success Criteria
1. Clear starting platform navigation
2. Successful BirdRider usage
3. Reach goal platform
4. Maintain performance targets
5. Smooth difficulty curve

## Design Notes
- Level focuses on timing and precision
- BirdRider adds movement variety
- Platform spacing encourages exploration
- Clear visual progression path
- Balanced challenge progression 