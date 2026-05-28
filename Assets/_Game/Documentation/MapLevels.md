# MapLevels Documentation

## Overview
This document describes the structure and components of MapLevel prefabs in the game. Each MapLevel is a prefab that contains the complete setup for a game level, including player spawn points, obstacles, and goals.

## Common Components

### 1. Root Object (MapLevel_X)
- **Name**: MapLevel_X (where X is the level number)
- **Components**:
  - Transform
  - LevelManager (MonoBehaviour)
- **Properties**:
  - topLeft: Reference to TopLeft limit marker
  - bottomRight: Reference to BottomRight limit marker
  - theme: Level theme number
  - _backGround: Reference to background object

### 2. Core Elements
- **Player Spawn Point**
  - Position: Varies by level
  - Tag: "Player"
  - Components: Player script

- **Ball**
  - Position: Near player spawn
  - Tag: "Ball"
  - Components: Ball script

- **Goal**
  - Position: Far right side
  - Tag: "Goal"
  - Components: Goal script

### 3. Camera Limits
- **TopLeft**
  - Position: (-10, 6)
  - Tag: "TopLeft"
  - Purpose: Defines camera boundary

- **BottomRight**
  - Position: (10, -6)
  - Tag: "BottomRight"
  - Purpose: Defines camera boundary

### 4. Level Sections
Each level is divided into sections:

#### Starting Area
- Contains basic platforms
- Player spawn point
- Initial obstacles
- StaticSquare for wind effects

#### Mid-Section
- Moving obstacles (Slime)
- Ice platforms
- SpiningPile obstacles
- Ground platforms

#### Advanced Section
- BigBall obstacles
- Do_BirdRider obstacles
- Complex platform arrangements
- Wind effects

#### Final Approach
- Combined obstacles
- Challenging platform layout
- Goal area setup

## Level Progression

### Difficulty Scaling
1. **Levels 1-10**
   - Basic platforming
   - Simple obstacles
   - Limited wind effects

2. **Levels 11-20**
   - Introduction of moving obstacles
   - Basic wind mechanics
   - More complex platforming

3. **Levels 21-30**
   - Advanced obstacle combinations
   - Multiple wind effects
   - Challenging platform layouts

4. **Levels 31-40**
   - Complex obstacle patterns
   - Multiple moving elements
   - Strategic wind usage

5. **Levels 41-50**
   - Expert-level challenges
   - Multiple obstacle types
   - Complex wind mechanics

## Technical Details

### Prefab Structure
```
MapLevel_X
├── Player
├── Ball
├── Goal
├── TopLeft
├── BottomRight
├── GroundBoundaries
├── StartingArea
│   ├── Ground Platforms
│   └── StaticSquare
├── MidSection
│   ├── Slime
│   ├── Ice Platforms
│   └── SpiningPile
├── AdvancedSection
│   ├── BigBall
│   └── Do_BirdRider
└── FinalApproach
    └── Combined Obstacles
```

### Component References
- Each section maintains references to its child objects
- Obstacles are properly tagged and layered
- Physics materials are assigned appropriately
- Collision layers are set correctly

## Best Practices

### Level Design
1. Start with basic structure
2. Add core gameplay elements
3. Place obstacles strategically
4. Test player movement
5. Verify ball physics
6. Check camera coverage
7. Add decorative elements
8. Final testing and optimization

### Performance Considerations
1. Optimize obstacle counts
2. Balance physics calculations
3. Manage draw calls
4. Consider memory usage
5. Test on target devices

### Testing Points
1. Player movement
2. Ball physics
3. Obstacle interactions
4. Camera boundaries
5. Performance metrics
6. Collision detection
7. Wind effects
8. Level completion 