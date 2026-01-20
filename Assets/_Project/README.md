# Precision Platformer Template
**CMGT 326 - Controls & Feel**

## Course Overview

This template provides the foundation for building a Celeste-quality precision platformer over 3 weeks (5 sessions + presentation).

### Team Structure
- **Systems Designer**: Code-heavy, implements movement systems and architecture
- **Gameplay Designer**: Feel-heavy, tunes parameters and tests in dev playground

### Core Philosophy
**CODE FIRST**: This template includes a complete professional dev playground so you can start coding immediately. Focus on systems implementation and feel tuning, not level building.

---

## What's Included

### ✅ Provided Infrastructure
- **Core Architecture**:
  - `ServiceLocator.cs` - Dependency injection pattern
  - `EventBus.cs` - Decoupled event system
  - `Bootstrap.cs` - Service initialization

- **Dev Playground Scene** (`DevPlayground.unity`):
  - 10 professional testing zones
  - All zones labeled and color-coded
  - Ready for immediate use

- **Input System Foundation**:
  - `InputHandler.cs` - Scaffold for input reading
  - InputActions configured for keyboard + gamepad

- **ScriptableObject Templates**:
  - `ConfigBase.cs` - Base class pattern
  - `MovementConfig.cs` - Example configuration

- **Platform Prefabs**:
  - BasicPlatform, Platform_Wide, Platform_Small
  - Wall_Vertical, Ramp prefabs (15°, 30°, 45°, 60°)

### ⚙️ What You Build (5 Sessions)

| Session | Systems Designer | Gameplay Designer |
|---------|-----------------|-------------------|
| **1** | Movement, ground detection, physics service | Tune movement feel in Zone A & B |
| **2** | Variable jump height, gravity scaling, multi-jump | Tune jump arcs in Zone C & D |
| **3** | Dash system, wall mechanics, state machine | Test wall climbs in Zone E, combos in Zone H |
| **4** | Coyote time, input buffering, forgiveness systems | Tune forgiveness windows in Zone G |
| **5** | Debug tools, metrics collection, level elements | Playtest and data-driven tuning |

---

## Dev Playground Zones

The playground is your **testing laboratory**. Each zone validates specific mechanics:

### Zone A: Horizontal Movement Test Strip (Red)
- **Purpose**: Measure run speed, acceleration, deceleration
- **Usage**: Time 0-to-max speed runs, test snap turns
- **Session 1**: Primary testing area for MovementController

### Zone B: Variable Platform Size Testing (Orange)
- **Purpose**: Validate ground detection on edge cases
- **Usage**: Walk across all platform widths (10u → 0.5u)
- **Session 1**: Ensure GroundDetectionService works reliably

### Zone C: Jump Height Calibration Towers (Blue)
- **Purpose**: Measure min/max jump heights
- **Usage**: Short tap vs. held jump, mark achievable heights
- **Session 2**: Tune JumpConfig parameters

### Zone D: Gap Distance Testing (Yellow)
- **Purpose**: Test jump distance and dash distance
- **Usage**: Find max reachable gap without dash, with dash
- **Session 2-3**: Validate jump + dash distances

### Zone E: Wall Jump Corridor (Green)
- **Purpose**: Test wall slide, wall jump, vertical climbing
- **Usage**: Climb from bottom to top using wall jumps
- **Session 3**: Validate WallInteractionController

### Zone F: Slope Testing Ramps (Brown)
- **Purpose**: Validate ground detection on angled surfaces
- **Usage**: Walk up/down ramps, verify grounded state
- **Session 1**: Ensure slopes don't break ground detection

### Zone G: Precision Jump Challenge Strip (Orange)
- **Purpose**: Test coyote time and input buffering
- **Usage**: Attempt jumps with late/early button presses
- **Session 4**: Measure forgiveness system effectiveness

### Zone H: Movement Chain Testing Area (Purple)
- **Purpose**: Test complex movement sequences
- **Usage**: Practice dash → wall jump → double jump chains
- **Session 3-5**: Validate movement chaining feels smooth

### Zone I: Performance Stress Test Zone (Red)
- **Purpose**: Stress test VFX and particle systems
- **Usage**: Spam jumps/dashes to create many particles
- **Session 4-5**: Ensure 60 FPS with heavy VFX

### Zone J: Free Experimentation Sandbox (Cyan)
- **Purpose**: Freestyle testing and custom challenges
- **Usage**: Add your own obstacles, test ideas
- **All Sessions**: Open creative space

---

## Architecture Patterns

### ServiceLocator Pattern
Register services at startup, access anywhere:

```csharp
// In Bootstrap.cs
var groundDetection = new GroundDetectionService();
ServiceLocator.Register<GroundDetectionService>(groundDetection);

// In MovementController.cs
var groundDetection = ServiceLocator.Get<GroundDetectionService>();
bool isGrounded = groundDetection.IsGrounded();
```

### EventBus Pattern
Publish events for decoupled communication:

```csharp
// In JumpController.cs
EventBus.Publish(new JumpStartedEvent(Time.time));

// In FeedbackManager.cs
void OnEnable() => EventBus.Subscribe<JumpStartedEvent>(OnJumpStarted);
void OnDisable() => EventBus.Unsubscribe<JumpStartedEvent>(OnJumpStarted);

void OnJumpStarted(JumpStartedEvent evt)
{
    // Play jump particle effect
}
```

### ScriptableObject Pattern
Create tunable configs for rapid iteration:

```csharp
[CreateAssetMenu(fileName = "JumpConfig", menuName = "Platformer/Config/Jump Config")]
public class JumpConfig : ConfigBase
{
    public float jumpForce = 15f;
    public float gravityMultiplierRising = 1f;
    public float gravityMultiplierFalling = 2f;
}
```

**Gameplay Designer** can tune these values in Inspector without touching code!

---

## Getting Started

### Session 1 Checklist

**Systems Designer:**
- [ ] Read `ServiceLocator.cs` and `EventBus.cs` to understand patterns
- [ ] Implement `GroundDetectionService.cs` with multi-raycast ground checking
- [ ] Implement `MovementController.cs` for horizontal movement
- [ ] Create `GroundDetectionConfig.cs` ScriptableObject
- [ ] Register services in `Bootstrap.cs`
- [ ] Test in Zone A (Movement Test Strip)

**Gameplay Designer:**
- [ ] Familiarize with Dev Playground - visit all 10 zones
- [ ] Create MovementConfig asset in `Configs/Movement/`
- [ ] Tune `maxSpeed`, `acceleration`, `deceleration` in Zone A
- [ ] Test ground detection reliability in Zone B (all platform sizes)
- [ ] Document baseline metrics:
  - 0-to-max speed time: ___ seconds
  - Stopping distance: ___ units
  - Smallest platform that feels safe: ___ units

### Important Files to Start With
1. `Assets/_Project/Scripts/Core/ServiceLocator.cs` - Understand this first
2. `Assets/_Project/Scripts/Core/EventBus.cs` - Understand this second
3. `Assets/_Project/Scripts/Input/InputHandler.cs` - Extend this for buffering later
4. `Assets/_Project/SETUP_GUIDE.md` - Manual Unity setup steps
5. `Assets/_Project/DEV_PLAYGROUND_GUIDE.md` - Zone usage reference

---

## Best Practices

### Code Organization
- **Namespace everything**: `PrecisionPlatformer.Core`, `PrecisionPlatformer.Systems`, etc.
- **Follow naming conventions**: Classes = PascalCase, private fields = camelCase
- **Add XML comments**: Every public method and property
- **Use [Header] and [Tooltip]**: Make Inspector user-friendly for designers

### Decentralized Architecture
✅ **DO**:
- Use EventBus for cross-system communication
- Access services via ServiceLocator
- Put all tunable values in ScriptableObjects
- Separate concerns (one class, one responsibility)

❌ **DON'T**:
- Create MonoBehaviour singletons (except Bootstrap)
- Use `GameObject.Find()` in Update loops
- Hardcode values - use ScriptableObjects!
- Tightly couple systems with direct references

### Testing Workflow
1. **Write code** (Systems Designer)
2. **Test in specific zone** (both designers)
3. **Tune parameters** (Gameplay Designer)
4. **Document results** (both designers)
5. **Iterate** based on feel

---

## Success Metrics

### Technical
- 60 FPS minimum at all times
- No MonoBehaviour singletons (except Bootstrap)
- All tunable values in ScriptableObjects
- Zero `GameObject.Find()` calls in Update

### Feel
- Controls feel "tight" and "responsive" (playtester feedback)
- Coyote time used in 20%+ of edge jumps (Session 4 metrics)
- Input buffer prevents 30%+ of mistimed jumps (Session 4 metrics)
- Movement chaining feels rewarding, not janky

---

## Resources

### Inside This Project
- `SETUP_GUIDE.md` - Unity Editor setup instructions
- `DEV_PLAYGROUND_GUIDE.md` - Zone-by-zone usage guide
- `ARCHITECTURE_OVERVIEW.md` - Deep dive into patterns
- `SESSION_CHECKLISTS/` - Week-by-week TODO lists

### External References
- [Celeste & Forgiveness by Maddy Thorson](https://maddythorson.medium.com/celeste-forgiveness-31e4a40399f1)
- [Unity Input System Documentation](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.16/manual/index.html)
- [ScriptableObject Architecture](https://unity.com/how-to/architect-game-code-scriptable-objects)

---

## Troubleshooting

### "Service not found" errors
- Ensure service is registered in `Bootstrap.InitializeServices()`
- Check spelling/capitalization matches exactly

### Player falls through platforms
- Verify platform Layer is set to "Ground" (Layer 6)
- Check Physics2D collision matrix in Project Settings
- Ensure Rigidbody2D Collision Detection = Continuous

### Input not working
- Check InputActionAsset is assigned to InputHandler
- Verify Input Actions are enabled in InputHandler.OnEnable()
- Open Input Actions asset - ensure Player action map has Move/Jump/Sprint

### Camera not following player
- Assign Player transform to CameraFollow.target in Inspector
- Or ensure Player has "Player" tag so CameraFollow auto-finds it

---

## Contact

Questions? Ask your instructor or post in class Discord.

**Ready to build Celeste-quality movement? Start in Zone A! 🎮**
