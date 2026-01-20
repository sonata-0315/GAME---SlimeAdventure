# Session 1 Checklist: Input Architecture & Grounded Movement

**Goal**: Implement horizontal movement with precise ground detection and tunable parameters.

---

## Pre-Session Setup

### Both Designers
- [ ] Read `README.md` completely
- [ ] Read `DEV_PLAYGROUND_GUIDE.md` - Zone A and Zone B sections
- [ ] Explore DevPlayground scene - visit all 10 zones
- [ ] Understand ServiceLocator and EventBus patterns (read the scripts)
- [ ] Set up version control (Git) if not already done

---

## Systems Designer Tasks

### 1. Understand Existing Architecture (30 min)
- [ ] Read `ServiceLocator.cs` - understand Register/Get pattern
- [ ] Read `EventBus.cs` - understand Subscribe/Publish pattern
- [ ] Read `Bootstrap.cs` - see where services are registered
- [ ] Read `InputHandler.cs` - understand how input events are published
- [ ] Test: Run the project, verify Bootstrap initializes correctly

### 2. Implement GroundDetectionService (1.5 hours)

**File**: `Assets/_Project/Scripts/Systems/GroundDetectionService.cs`

**Requirements**:
- Multi-raycast ground checking (3-5 raycasts across player width)
- Returns: `bool isGrounded`, `Vector3 surfaceNormal`, `float slopeAngle`
- Uses `GroundDetectionConfig` ScriptableObject for parameters
- Layer mask filtering (only detect Ground layer)
- Visualize raycasts with `Debug.DrawRay()` for debugging

**Checklist**:
- [ ] Create `GroundDetectionService.cs`
- [ ] Implement multi-raycast logic (center, left, right minimum)
- [ ] Add slope angle calculation using surface normal
- [ ] Add configurable raycast count, spacing, distance
- [ ] Test in Zone B - should detect all platform sizes reliably

### 3. Create GroundDetectionConfig ScriptableObject (15 min)

**File**: `Assets/_Project/Scripts/Data/GroundDetectionConfig.cs`

**Required Fields**:
```csharp
- raycastCount: int (default: 3)
- raycastSpacing: float (default: 0.4f)
- raycastDistance: float (default: 0.1f)
- groundLayerMask: LayerMask (set to Ground layer)
- slopeLimit: float (default: 45f)
```

**Checklist**:
- [ ] Create ScriptableObject script inheriting from `ConfigBase`
- [ ] Add `[CreateAssetMenu]` attribute
- [ ] Add validation in `Validate()` method
- [ ] Create asset in `Assets/_Project/Configs/Movement/`
- [ ] Fill in description field

### 4. Implement MovementController (2 hours)

**File**: `Assets/_Project/Scripts/Entities/Player/MovementController.cs`

**Requirements**:
- Subscribe to `InputMoveEvent` from EventBus
- Use `GroundDetectionService` via ServiceLocator
- Apply horizontal acceleration/deceleration from `MovementConfig`
- Publish `MovementStateChanged` event when speed changes significantly
- Modify `Rigidbody2D.velocity` in `FixedUpdate()`
- Support turn-around boost when changing directions

**Checklist**:
- [ ] Create `MovementController.cs` MonoBehaviour
- [ ] Subscribe to InputMoveEvent in OnEnable, unsubscribe in OnDisable
- [ ] Implement acceleration logic using `MovementConfig` parameters
- [ ] Implement deceleration with friction
- [ ] Add turn-around detection and boost
- [ ] Apply velocity to Rigidbody2D in FixedUpdate
- [ ] Test in Zone A - should accelerate smoothly

### 5. Create MovementStateChanged Event (15 min)

**File**: `Assets/_Project/Scripts/Events/MovementEvents.cs`

**Add Event**:
```csharp
public readonly struct MovementStateChangedEvent
{
    public readonly float CurrentSpeed;
    public readonly Vector2 Velocity;
    public readonly bool IsGrounded;
    public readonly float Timestamp;
}
```

**Checklist**:
- [ ] Add event struct to MovementEvents.cs
- [ ] Publish event from MovementController when state changes

### 6. Register Services in Bootstrap (15 min)

**File**: `Assets/_Project/Scripts/Core/Bootstrap.cs`

**Add to `InitializeServices()`**:
```csharp
var groundDetection = new GroundDetectionService();
ServiceLocator.Register<GroundDetectionService>(groundDetection);
```

**Checklist**:
- [ ] Register GroundDetectionService
- [ ] Verify services initialize in correct order
- [ ] Test: Check console for initialization logs

### 7. Test and Debug (30 min)
- [ ] Attach MovementController to Player in DevPlayground scene
- [ ] Assign MovementConfig to MovementController in Inspector
- [ ] Play scene - Player should move left/right
- [ ] Verify ground detection works in Zone B (all platform sizes)
- [ ] Fix any bugs or issues

---

## Gameplay Designer Tasks

### 1. Create MovementConfig Asset (15 min)
- [ ] Right-click in `Assets/_Project/Configs/Movement/`
- [ ] Create → Platformer → Config → Movement Config
- [ ] Rename to "DefaultMovementConfig"
- [ ] Fill description: "Baseline movement config for Session 1"
- [ ] Leave default values initially

### 2. Create GroundDetectionConfig Asset (15 min)
- [ ] Right-click in `Assets/_Project/Configs/Movement/`
- [ ] Create → Platformer → Config → Ground Detection Config
- [ ] Rename to "DefaultGroundDetection"
- [ ] Set `groundLayerMask` to Ground layer (Layer 6)
- [ ] Set `raycastCount` = 3
- [ ] Set `raycastDistance` = 0.15

### 3. Test in Zone A: Movement Test Strip (1 hour)

**Test Acceleration**:
- [ ] Time 0-to-max speed (target: 0.3-0.5 seconds)
- [ ] Document current time: ___ seconds
- [ ] Adjust `acceleration` if too slow/fast
- [ ] Retest until feels responsive

**Test Deceleration**:
- [ ] Measure stopping distance (target: 2-4 units)
- [ ] Document current distance: ___ units
- [ ] Adjust `deceleration` if too slippery/sticky
- [ ] Retest until feels grounded

**Test Turn-Around**:
- [ ] Run right at max speed, flip to left input
- [ ] Should feel instant, not delayed
- [ ] Adjust `turnAroundBoost` if sluggish (try 1.5-2.0)
- [ ] Retest until snap-turn feels good

### 4. Test in Zone B: Platform Sizes (45 min)

**Reliability Test**:
- [ ] Walk across each platform width (10u → 0.5u)
- [ ] Note if player falls through: Yes/No
- [ ] Smallest safe platform: ___ units

**Edge Detection Test**:
- [ ] Run at max speed across platforms
- [ ] Player should NOT fall off edges accidentally
- [ ] If falling off, increase `raycastCount` to 5

**Slope Test** (Zone F):
- [ ] Walk up/down each ramp (15°-60°)
- [ ] Player should stay grounded on all ramps
- [ ] Note steepest walkable angle: ___ degrees

### 5. Parameter Tuning Iteration (1 hour)

Create tuning document (Google Doc or spreadsheet):

| Parameter | Initial Value | Feels | New Value | Result |
|-----------|--------------|-------|-----------|--------|
| maxSpeed | 8.0 | Too slow | 10.0 | Better |
| acceleration | 40 | Floaty | 50 | Good! |
| ... | ... | ... | ... | ... |

**Iterate**:
- [ ] Change ONE parameter at a time
- [ ] Test in appropriate zone
- [ ] Document subjective feel rating (1-10)
- [ ] Repeat until movement feels "right"

### 6. Document Baseline Metrics (30 min)

**Create Metrics Document** with:
- 0-to-max speed time: ___ seconds
- Stopping distance: ___ units
- Turn-around responsiveness: Instant / Delayed / Sluggish
- Smallest safe platform: ___ units
- Ground detection reliability: Perfect / Good / Needs work
- Overall movement feel rating: __/10

---

## Team Integration (30 min)

### Both Designers Together
- [ ] Systems Designer explains implementation to Gameplay Designer
- [ ] Gameplay Designer demonstrates tuned parameters
- [ ] Discuss feel: too floaty? too sticky? just right?
- [ ] Agree on final MovementConfig values for baseline

### Code Review
- [ ] Gameplay Designer reviews code for understanding
- [ ] Systems Designer explains any tricky parts
- [ ] Both agree code is clean and well-commented

---

## Session 1 Deliverables

### Technical Deliverables
- [ ] `GroundDetectionService.cs` implemented and tested
- [ ] `MovementController.cs` implemented and tested
- [ ] `GroundDetectionConfig.cs` ScriptableObject created
- [ ] `MovementStateChangedEvent` added to MovementEvents.cs
- [ ] Services registered in Bootstrap
- [ ] Player moves smoothly in DevPlayground

### Design Deliverables
- [ ] DefaultMovementConfig tuned with documented values
- [ ] DefaultGroundDetectionConfig tuned for reliability
- [ ] Baseline metrics documented
- [ ] Tuning iteration log (what changed and why)

### Documentation
- [ ] Code has XML comments on all public methods
- [ ] Configs have helpful [Tooltip] descriptions
- [ ] Metrics document shared with instructor

---

## Common Issues & Solutions

### Issue: Player falls through platforms
**Solution**:
- Increase `raycastDistance` in GroundDetectionConfig
- Verify platforms are on Ground layer (Layer 6)
- Check Physics2D collision matrix includes Player ↔ Ground

### Issue: Movement feels floaty/unresponsive
**Solution**:
- Increase `acceleration` (try 50-60)
- Decrease `airControlMultiplier` (try 0.6-0.7)
- Check Rigidbody2D.gravityScale is 3 or higher

### Issue: Input doesn't work
**Solution**:
- Verify InputHandler has InputActionAsset assigned
- Check Input Actions are enabled in InputHandler.OnEnable
- Ensure InputHandler is attached to a GameObject in scene

### Issue: Player slides forever (ice-skating)
**Solution**:
- Increase `deceleration` (try 60-80)
- Increase `frictionCoefficient` (try 0.98-1.0)
- Apply friction every frame in MovementController

---

## Next Session Preview

**Session 2**: Jump Mechanics & Variable Jump Height
- Implement `JumpController.cs` with hold-duration jump
- Create gravity scaling system (rising/apex/falling)
- Test in Zone C (Jump Height Towers)
- Tune jump arc for Celeste-style "weighted" feel

**Prepare for Session 2**:
- Read about variable jump height implementation
- Study Celeste's jump feel analysis
- Think about min/max jump heights you want

---

## Time Budget (Total: ~4-6 hours)

| Task | Systems Designer | Gameplay Designer |
|------|-----------------|-------------------|
| Architecture understanding | 30 min | 30 min |
| Implementation | 4 hours | - |
| Config creation | - | 30 min |
| Testing & tuning | 30 min | 3 hours |
| Documentation | 30 min | 30 min |
| Team integration | 30 min | 30 min |
| **TOTAL** | **6 hours** | **4.5 hours** |

**Pace yourself!** This is challenging work. Take breaks, ask questions, and iterate until it feels right.

---

## Success = Movement Feels Tight and Responsive

If at the end of Session 1:
- ✅ Player moves smoothly left/right
- ✅ Ground detection never fails
- ✅ Parameters are tunable in Inspector
- ✅ Movement feels "tight" not "floaty"

**YOU'RE READY FOR SESSION 2! 🎮**
