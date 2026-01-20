# Dev Playground Usage Guide

This guide explains how to use each testing zone effectively for mechanics validation and parameter tuning.

---

## Philosophy: Testing Laboratory, Not a Level

The Dev Playground is **NOT a gameplay level**. It's a systematic testing environment where you validate mechanics in isolation before combining them.

Think of it like a scientist's lab bench - each zone tests one hypothesis, collects data, and informs tuning decisions.

---

## Zone A: Horizontal Movement Test Strip

### What It Tests
- Maximum run speed
- Acceleration time (0 to max)
- Deceleration distance
- Turn-around snap responsiveness

### How to Use
1. **Acceleration Test**:
   - Stand at leftmost marker
   - Hold right input
   - Count seconds to reach max speed
   - Target: 0.3-0.5 seconds feels responsive

2. **Deceleration Test**:
   - Run at max speed
   - Release input
   - Count units until full stop
   - Target: 2-4 units feels grounded (not ice-skating)

3. **Turn-Around Test**:
   - Run right at max speed
   - Quickly flip to left input
   - Should feel instant, not sluggish
   - Tune `turnAroundBoost` if it feels delayed

### Parameters to Tune (MovementConfig)
- `maxSpeed`: Increase if feels too slow
- `acceleration`: Increase if feels floaty/unresponsive
- `deceleration`: Increase if feels slippery
- `turnAroundBoost`: Increase if direction changes feel delayed

### Session 1 Deliverable
Document these metrics:
- 0-to-max time: ___ seconds
- Stop distance: ___ units
- Turn-around feels: Instant / Slightly delayed / Too slow

---

## Zone B: Variable Platform Size Testing

### What It Tests
- Ground detection reliability on different platform widths
- Edge detection accuracy
- When player should/shouldn't fall off

### How to Use
1. **Walk Test**:
   - Walk slowly across each platform
   - Note if player falls through or floats

2. **Run Test**:
   - Run across at max speed
   - Edge detection should prevent accidental falls

3. **Jump Landing Test**:
   - Jump onto platforms from above
   - Should land reliably on all except maybe 0.5u

### Parameters to Tune (GroundDetectionConfig)
- `raycastCount`: Increase if missing ground on small platforms
- `raycastSpacing`: Adjust based on player width
- `raycastDistance`: Increase if not detecting ground reliably

### Session 1 Deliverable
Document:
- Smallest platform width that feels safe: ___ units
- Do you fall through any platforms? Yes/No
- Edge detection prevents falls? Yes/No

---

## Zone C: Jump Height Calibration Towers

### What It Tests
- Minimum jump height (quick tap)
- Maximum jump height (held button)
- Jump arc shape and predictability

### How to Use
1. **Min Height Test**:
   - Stand at 0u platform
   - Tap jump button as briefly as possible
   - Mark highest platform reached
   - Target: 2-3 units for precise control

2. **Max Height Test**:
   - Hold jump button fully
   - Mark highest platform reached
   - Target: 6-8 units for gameplay variety

3. **Variable Height Test**:
   - Practice reaching specific heights (4u, 5u, 6u)
   - Should feel controllable, not random

### Parameters to Tune (JumpConfig - Session 2)
- `jumpForce`: Increase if max height too low
- `minJumpHeight`: Tune for tap-jump control
- `gravityMultiplierRising`: Decrease for floatier ascent
- `gravityMultiplierFalling`: Increase for snappier descent

### Session 2 Deliverable
Document:
- Min jump height (tap): ___ units
- Max jump height (held): ___ units
- Jump arc feels: Floaty / Snappy / Weighted / Perfect

---

## Zone D: Gap Distance Testing

### What It Tests
- Maximum horizontal jump distance
- Dash distance extension
- Combined jump + dash reach

### How to Use
1. **Jump Distance Test (Session 2)**:
   - Jump from edge of platform
   - Mark farthest gap cleared with jump alone
   - Target: 5-6 units feels good

2. **Dash Distance Test (Session 3)**:
   - Dash across gaps
   - Mark farthest gap cleared with dash alone
   - Target: 7-8 units

3. **Combined Test (Session 3)**:
   - Jump + dash in air
   - Mark maximum combined reach
   - Target: 9-10 units for advanced movement

### Parameters to Tune
- `jumpForce` (JumpConfig): Affects horizontal distance
- `dashSpeed` and `dashDuration` (DashConfig - Session 3)
- `airControlMultiplier` (MovementConfig): Affects mid-air steering

### Session 2-3 Deliverable
Document:
- Max jump distance: ___ units
- Max dash distance: ___ units
- Max jump+dash distance: ___ units

---

## Zone E: Wall Jump Corridor

### What It Tests
- Wall slide friction
- Wall jump trajectory
- Vertical climbing capability

### How to Use
1. **Wall Slide Test (Session 3)**:
   - Jump into wall and hold toward it
   - Should slide down smoothly, not fall instantly
   - Target: 2-3 seconds to slide full height

2. **Wall Jump Test**:
   - Jump off wall
   - Should push away from wall clearly
   - Input direction should influence trajectory

3. **Climb Test**:
   - Climb from bottom to top using wall jumps
   - Should be possible but require skill
   - Target: 3-4 wall jumps to reach top

### Parameters to Tune (WallInteractionConfig - Session 3)
- `wallSlideSpeed`: Decrease for slower slide
- `wallJumpForce`: Increase if can't climb effectively
- `wallJumpInputInfluence`: Adjust player control vs. fixed trajectory
- `wallStickTime`: Brief pause before slide feels good

### Session 3 Deliverable
Document:
- Can climb to top? Yes/No
- Wall jumps required: ___
- Wall slide feels: Too fast / Too slow / Just right

---

## Zone F: Slope Testing Ramps

### What It Tests
- Ground detection on angled surfaces
- Movement consistency on slopes
- Slope angle limits

### How to Use
1. **Walk Up/Down Test**:
   - Walk up each ramp slowly
   - Should stay grounded (not pop off)

2. **Run Up/Down Test**:
   - Run at max speed
   - Should maintain ground contact

3. **Jump on Slopes Test**:
   - Jump while on ramp
   - Should jump perpendicular to slope or vertically

### Parameters to Tune (GroundDetectionConfig)
- `slopeLimit`: Maximum angle considered "ground"
- Might need to adjust raycast angles for slopes

### Session 1 Deliverable
Document:
- All slopes detected as ground? Yes/No
- Player pops off slopes? Yes/No
- Steepest walkable angle: ___ degrees

---

## Zone G: Precision Jump Challenge Strip

### What It Tests
- Coyote time effectiveness
- Input buffering reliability
- Corner correction feel

### How to Use
1. **Late Jump Test (Session 4 - Coyote Time)**:
   - Run off platform edge
   - Press jump AFTER leaving platform
   - Should still jump (grace period)
   - Compare with/without coyote time

2. **Early Jump Test (Session 4 - Input Buffer)**:
   - Press jump BEFORE landing on platform
   - Should jump immediately upon landing
   - Compare with/without buffering

3. **Near-Miss Test (Session 4 - Corner Correction)**:
   - Jump slightly short of platform
   - Should nudge onto platform if close
   - Should feel helpful, not intrusive

### Parameters to Tune (Session 4)
- `coyoteTimeDuration` (CoyoteTimeConfig): 0.05-0.15s typical
- `jumpBufferDuration` (InputBufferConfig): 0.1-0.2s typical
- `correctionDistance` (CornerCorrectionConfig): 2-5 pixels

### Session 4 Deliverable
Document:
- Coyote time window: ___ seconds (feels right)
- Input buffer window: ___ seconds
- Forgiveness makes "impossible" jumps possible? Yes/No
- Metrics: Coyote time activated ___% of edge jumps (collect via MetricsCollector)

---

## Zone H: Movement Chain Testing Area

### What It Tests
- Complex movement sequences
- Smooth transitions between mechanics
- Advanced movement flow

### How to Use
1. **Basic Chains (Session 3)**:
   - Run → Jump → Land
   - Run → Jump → Dash → Land
   - Wall jump → Double jump

2. **Advanced Chains (Session 4-5)**:
   - Dash → Wall jump → Dash → Wall jump (alternating walls)
   - Ground → Jump → Wall slide → Wall jump → Dash → Land
   - Test your most complex combos

3. **Flow Testing**:
   - Do transitions feel smooth or janky?
   - Can you maintain momentum?
   - Are there awkward pauses or stalls?

### Parameters to Tune
- `dashEndLagDuration` (DashConfig): Reduce if transitions feel sluggish
- Movement state machine priorities
- Input buffering between different actions

### Session 3-5 Deliverable
Document:
- Most complex successful chain: _______________
- Transitions feel: Smooth / Slightly janky / Very janky
- Momentum preservation: Good / Inconsistent / Poor

---

## Zone I: Performance Stress Test Zone

### What It Tests
- Frame rate with heavy VFX
- Object pooling efficiency
- Performance degradation

### How to Use
1. **Particle Spam Test (Session 4-5)**:
   - Jump repeatedly to spawn many particles
   - Dash repeatedly
   - Monitor frame rate (target: 60 FPS minimum)

2. **Pool Exhaustion Test**:
   - Spawn max particles
   - Verify pooling reuses objects (no new allocations)

3. **Memory Profiling**:
   - Use Unity Profiler
   - Check for memory spikes or garbage collection

### Parameters to Tune (ParticlePoolConfig - Session 4)
- `poolSizePerType`: Increase if running out of pooled objects
- Particle lifetime: Reduce if too many active at once

### Session 4-5 Deliverable
Document:
- Frame rate with heavy VFX: ___ FPS
- Pooling working correctly? Yes/No
- GC spikes observed? Yes/No

---

## Zone J: Free Experimentation Sandbox

### What It Tests
- Whatever you want!

### How to Use
1. **Build Custom Challenges**:
   - Add platforms, walls, gaps as needed
   - Test ideas for your final challenge levels

2. **Practice Advanced Techniques**:
   - Practice movement chains
   - Test edge cases
   - Experiment with parameter extremes

3. **Creative Exploration**:
   - Try "broken" configs to understand limits
   - Discover unintended movement options

### No Deliverable
This is YOUR space. Use it however helps your development process.

---

## General Testing Workflow

### For Every Mechanic:
1. **Implement System** (Systems Designer)
2. **Test in Specific Zone** (both designers together)
3. **Document Baseline** (both designers)
4. **Tune Parameters** (Gameplay Designer leads)
5. **Retest and Compare** (both designers)
6. **Iterate Until "Feels Right"** (both designers)

### "Feels Right" Checklist:
- ✅ Responsive (input → action feels instant)
- ✅ Predictable (same input = same result)
- ✅ Controllable (player feels in control)
- ✅ Forgiving (minor mistakes don't feel unfair)
- ✅ Fun (testing is enjoyable, not frustrating)

---

## Metrics to Track

Create a spreadsheet with:

| Zone | Mechanic | Parameter | Value | Feel Rating | Notes |
|------|----------|-----------|-------|-------------|-------|
| A | Movement | maxSpeed | 8.0 | Good | Feels responsive |
| A | Movement | acceleration | 40 | Too slow | Increase to 50 |
| C | Jump | jumpForce | 15 | Good | Nice arc |
| ... | ... | ... | ... | ... | ... |

This data informs your Session 5 playtesting analysis and final presentation.

---

## Remember

**The playground is NOT a level to beat - it's a scientific instrument.**

- Every test should answer a specific question
- Every parameter change should be intentional
- Every "feel" judgment should be compared to baseline
- Document everything - data drives design!

