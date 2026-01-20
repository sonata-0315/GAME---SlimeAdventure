# Platformer Template Setup Guide

This guide explains how to complete the template setup in Unity Editor.

## Table of Contents
1. [Create Scenes](#create-scenes)
2. [Create Platform Prefabs](#create-platform-prefabs)
3. [Create Player Prefab](#create-player-prefab)
4. [Build DevPlayground Scene](#build-devplayground-scene)
5. [Create Config Assets](#create-config-assets)

---

## 1. Create Scenes

### BootstrapScene
1. Delete `Assets/Scenes/SampleScene.unity`
2. Create new scene: `File → New Scene` → `2D (Built-in)`
3. Save as `Assets/_Project/Scenes/BootstrapScene.unity`
4. In Hierarchy, create empty GameObject: `Create → Create Empty`
5. Rename to "ServiceLocatorBootstrap"
6. Add `Bootstrap.cs` component to it
7. In Bootstrap inspector, set "Scene To Load" to "DevPlayground"
8. Save scene

### DevPlayground Scene
1. Create new scene: `File → New Scene` → `2D (Built-in)`
2. Save as `Assets/_Project/Scenes/DevPlayground.unity`
3. Set Main Camera to Orthographic Size: **20** (to see entire playground)
4. Add `CameraFollow.cs` component to Main Camera
5. Set CameraFollow offset to (0, 0, -10)
6. Save scene

---

## 2. Create Platform Prefabs

### BasicPlatform.prefab (1x1 unit)
1. In DevPlayground scene, create: `GameObject → 2D Object → Sprite → Square`
2. Rename to "BasicPlatform"
3. Set Transform Position: (0, 0, 0)
4. Set Transform Scale: (1, 1, 1)
5. In SpriteRenderer:
   - Color: White (255, 255, 255)
   - Sorting Layer: Default
6. Add Component: `Box Collider 2D`
   - Size: (1, 1)
7. Set Layer to "Ground" (Layer 6)
8. Set Tag to "Ground"
9. Drag from Hierarchy to `Assets/_Project/Prefabs/Platforms/` to create prefab
10. Delete from scene

### Platform_Wide.prefab (10x1 unit)
1. Duplicate BasicPlatform prefab
2. Rename to "Platform_Wide"
3. Open prefab (double-click in Project)
4. Set Transform Scale: **(10, 1, 1)**
5. In BoxCollider2D, set Size: **(10, 1)**
6. Save prefab

### Platform_Small.prefab (0.5x1 unit)
1. Duplicate BasicPlatform prefab
2. Rename to "Platform_Small"
3. Open prefab
4. Set Transform Scale: **(0.5, 1, 1)**
5. In BoxCollider2D, set Size: **(0.5, 1)**
6. Save prefab

### Wall_Vertical.prefab (1x10 unit)
1. Duplicate BasicPlatform prefab
2. Rename to "Wall_Vertical"
3. Open prefab
4. Set Transform Scale: **(1, 10, 1)**
5. In BoxCollider2D, set Size: **(1, 10)**
6. Set Layer to "Wall" (Layer 7)
7. Set Tag to "Wall"
8. Set SpriteRenderer Color: Green (0, 255, 0) - for visibility
9. Save prefab

### Ramp Prefabs (15°, 30°, 45°, 60°)

**Ramp_15deg.prefab:**
1. Create: `GameObject → 2D Object → Sprite → Triangle`
2. Rename to "Ramp_15deg"
3. Set Transform Rotation Z: **15**
4. Set Transform Scale: (2, 0.5, 1)
5. Remove SpriteRenderer, add `Sprite Renderer` with Triangle sprite
6. Add Component: `Polygon Collider 2D` (auto-fits to sprite)
7. Set Layer: Ground
8. Set Tag: Ground
9. Create prefab in `Assets/_Project/Prefabs/Platforms/`

**Ramp_30deg, 45deg, 60deg:**
- Follow same steps, changing rotation to 30, 45, 60 degrees
- Adjust scale as needed for visual consistency

### ZoneLabel Prefab
1. In DevPlayground scene: `GameObject → UI → Text - TextMeshPro`
2. If prompted, import TMP Essentials
3. Rename to "ZoneLabel"
4. In RectTransform:
   - Width: 200
   - Height: 50
5. In TextMeshPro component:
   - Font Size: 24
   - Alignment: Center
   - Color: White
   - Enable Auto Size: false
6. Create prefab in `Assets/_Project/Prefabs/`
7. Delete from scene

---

## 3. Create Player Prefab

1. In DevPlayground scene, create: `GameObject → 2D Object → Sprite → Square`
2. Rename to "Player"
3. Set Transform Position: **(0, -15, 0)** (spawn point)
4. Set Transform Scale: (1, 1, 1)
5. In SpriteRenderer:
   - Color: **Cyan (0, 255, 255)** (high visibility)
6. Add Component: `Rigidbody2D`
   - Body Type: Dynamic
   - Mass: 1
   - Linear Drag: 0
   - Angular Drag: 0
   - Gravity Scale: 3 (will be controlled by PhysicsService later)
   - Collision Detection: Continuous
   - Sleeping Mode: Never Sleep
   - Interpolation: Interpolate
   - **Constraints: Freeze Rotation Z** ✓ (IMPORTANT!)
7. Add Component: `Box Collider 2D`
   - Size: (0.9, 0.9) - slightly smaller for forgiving collisions
8. Set Layer: Player (Layer 3)
9. Set Tag: Player
10. Create prefab in `Assets/_Project/Prefabs/`
11. **Keep instance in DevPlayground scene** (don't delete)

---

## 4. Build DevPlayground Scene

Open DevPlayground.unity and build all 10 testing zones.

### Scene Layout Overview
Arrange zones in a 2D grid:
```
[Zone A: Movement Test]  [Zone B: Platform Sizes]  [Zone C: Jump Towers]
[Zone D: Gap Testing]    [Zone E: Wall Corridor]   [Zone F: Slopes]
[Zone G: Precision]      [Zone H: Combo Testing]   [Zone I: Performance]
                         [Zone J: Sandbox]
```

Position zones with ~20 unit spacing between them.

---

### ZONE A: Horizontal Movement Test Strip
**Position: (-40, -15, 0)**

1. Create Platform_Wide instance
2. Position at (-40, -15, 0)
3. Set Scale: (30, 2, 1) - very long platform
4. Add distance markers every 2 units:
   - Create small cubes (0.1 x 0.5) at x positions: -40, -38, -36, ..., -10
   - Color them yellow for visibility
5. Create ZoneLabel above at (-25, -10, 0)
   - Text: "ZONE A: MOVEMENT TEST\n0-to-max speed measurement"
   - Color: Red (255, 0, 0)
6. Create colored background quad (optional):
   - Sprite: Square, Scale (35, 10, 1)
   - Color: Dark Red (100, 0, 0, 50) - semi-transparent
   - Position behind platforms (Z = 1)

---

### ZONE B: Variable Platform Size Testing
**Position: (-10, -15, 0)**

1. Create platforms with decreasing widths:
   - Platform 1: Scale (10, 1, 1) at (-10, -15, 0) - label "10u"
   - Platform 2: Scale (8, 1, 1) at (1, -15, 0) - label "8u"
   - Platform 3: Scale (6, 1, 1) at (10, -15, 0) - label "6u"
   - Platform 4: Scale (4, 1, 1) at (17, -15, 0) - label "4u"
   - Platform 5: Scale (2, 1, 1) at (22, -15, 0) - label "2u"
   - Platform 6: Scale (1, 1, 1) at (24.5, -15, 0) - label "1u"
   - Platform 7: Scale (0.5, 1, 1) at (26, -15, 0) - label "0.5u"
2. Gaps of 1 unit between platforms
3. Create ZoneLabel at (8, -10, 0)
   - Text: "ZONE B: GROUND DETECT\nEdge case platform widths"
   - Color: Orange (255, 165, 0)

---

### ZONE C: Jump Height Calibration Towers
**Position: (30, -15, 0)**

1. Create vertical tower of platforms every 1 unit height:
   - Platform at height 0: (30, -15, 0) - label "0u"
   - Platform at height 1: (30, -14, 0) - label "1u"
   - Platform at height 2: (30, -13, 0) - label "2u"
   - ... continue to height 10
   - Each platform: Scale (3, 0.3, 1) - thin horizontal ledges
2. Create second tower at x=35 for reference
3. Create ZoneLabel at (32, -5, 0)
   - Text: "ZONE C: JUMP HEIGHT\nMin/Max calibration"
   - Color: Blue (0, 100, 255)

---

### ZONE D: Gap Distance Testing
**Position: (-40, 5, 0)**

1. Create platforms with increasing gaps:
   - Platform 1 at (-40, 5, 0), Scale (3, 1, 1)
   - Gap 2u → Platform 2 at (-35, 5, 0) - label "2u gap"
   - Gap 3u → Platform 3 at (-29, 5, 0) - label "3u gap"
   - Gap 4u → Platform 4 at (-22, 5, 0) - label "4u gap"
   - Gap 5u → Platform 5 at (-14, 5, 0) - label "5u gap"
   - Gap 6u → Platform 6 at (-5, 5, 0) - label "6u gap"
   - Gap 7u → Platform 7 at (5, 5, 0) - label "7u gap"
   - Gap 8u → Platform 8 at (16, 5, 0) - label "8u gap"
2. Create ZoneLabel at (-17, 10, 0)
   - Text: "ZONE D: GAP JUMPS\nDistance validation"
   - Color: Yellow (255, 255, 0)

---

### ZONE E: Wall Jump Corridor
**Position: (30, 5, 0)**

1. Create left wall: Wall_Vertical at (28, 10, 0)
2. Create right wall: Wall_Vertical at (32, 10, 0)
3. Corridor is 4 units wide, 20 units tall
4. Add bottom platform at (30, 0, 0), Scale (6, 1, 1)
5. Add top platform at (30, 20, 0), Scale (6, 1, 1)
6. Create ZoneLabel at (30, 25, 0)
   - Text: "ZONE E: WALL CLIMB\nWall jump test corridor"
   - Color: Green (0, 255, 0)

---

### ZONE F: Slope Testing Ramps
**Position: (45, 5, 0)**

1. Create flat platform at (45, 5, 0), Scale (3, 1, 1)
2. Ramp 15° at (49, 5, 0) - going up
3. Flat platform at (52, 6, 0)
4. Ramp 30° at (55, 6, 0) - going up
5. Flat platform at (58, 8, 0)
6. Ramp 45° at (61, 8, 0) - going up
7. Flat platform at (64, 11, 0)
8. Ramp 60° at (67, 11, 0) - going up
9. Add downward ramps as mirror
10. Label each ramp with angle
11. Create ZoneLabel at (56, 17, 0)
    - Text: "ZONE F: SLOPES\nAngle detection: 15-60°"
    - Color: Brown (165, 42, 42)

---

### ZONE G: Precision Jump Challenge Strip
**Position: (-40, 25, 0)**

1. Create series of small platforms (1-2 units wide) with 1 unit gaps
2. Alternate heights: (0, +1, -1, +2, -1, +1, 0, +2, etc.)
3. Create 15-20 platforms in sequence
4. This tests coyote time and input buffering
5. Create ZoneLabel at (-20, 32, 0)
   - Text: "ZONE G: FORGIVENESS TEST\nCoyote time & buffer validation"
   - Color: Orange (255, 100, 0)

---

### ZONE H: Movement Chain Testing Area
**Position: (10, 25, 0)**

1. Create mixed obstacles:
   - Wall at (10, 30, 0) - Height 10
   - Platform at (15, 28, 0)
   - Gap 5 units
   - Wall at (22, 30, 0)
   - Platform at (15, 35, 0)
   - etc. - create parkour-like obstacle course
2. Design for testing:
   - Run → Jump → Dash → Land
   - Wall jump → Dash → Wall jump chains
3. Create ZoneLabel at (15, 40, 0)
   - Text: "ZONE H: COMBO TEST\nMovement chaining"
   - Color: Purple (200, 0, 255)

---

### ZONE I: Performance Stress Test Zone
**Position: (40, 25, 0)**

1. Create enclosed area: 10x10 box of walls
2. Fill with many small platforms
3. Purpose: spam jumps/dashes to create many particles for performance testing
4. Create ZoneLabel at (40, 37, 0)
   - Text: "ZONE I: PERFORMANCE\nVFX stress test"
   - Color: Red (255, 0, 0)

---

### ZONE J: Free Experimentation Sandbox
**Position: (0, 45, 0)**

1. Create large open space: 30x20 units
2. Add ~10 scattered platforms of various sizes
3. Add 2-3 walls
4. Keep it sparse - students customize this
5. Create ZoneLabel at (0, 58, 0)
   - Text: "ZONE J: SANDBOX\nFree experimentation area"
   - Color: Cyan (0, 255, 255)

---

### Final DevPlayground Setup

1. Set Player spawn position: (0, -15, 0) - near Zone A
2. Set Main Camera position: (0, 10, -10)
3. Add background grid sprite (optional):
   - Create large quad sprite
   - Position at (0, 20, 10) - behind everything
   - Scale to cover entire playground
   - Use subtle grid texture or solid dark gray
4. Set CameraFollow target to Player in scene
5. Save scene
6. Add DevPlayground to Build Settings: `File → Build Settings → Add Open Scenes`

---

## 5. Create Config Assets

### MovementConfig Asset
1. Right-click in `Assets/_Project/Configs/Movement/`
2. `Create → Platformer → Config → Movement Config`
3. Rename to "DefaultMovementConfig"
4. Leave default values (students will tune these)
5. Fill in description: "Default movement configuration for Session 1 tuning"

### Create Additional Config Placeholders
Students will create these in later sessions:
- JumpConfig (Session 2)
- DashConfig (Session 3)
- CoyoteTimeConfig (Session 4)
- InputBufferConfig (Session 4)

---

## 6. Final Build Settings

1. `File → Build Settings`
2. Add scenes in order:
   - BootstrapScene
   - DevPlayground
3. Set BootstrapScene as scene 0 (first)
4. Close Build Settings

---

## 7. Test the Template

1. Open BootstrapScene
2. Press Play
3. Verify:
   - DevPlayground loads
   - Player appears at spawn
   - Camera follows player
   - Console shows Bootstrap initialization messages
   - No errors

---

## Complete! Template is Ready for Students

Students can now:
- Start coding MovementController in Session 1
- Test in Zone A (Movement Test Strip)
- Tune MovementConfig values
- Use all zones for systematic testing

