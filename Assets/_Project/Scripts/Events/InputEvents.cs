using UnityEngine;

namespace PrecisionPlatformer.Events
{
    /// <summary>
    /// Event definitions for input system.
    /// Using readonly structs for zero-allocation event passing.
    /// Students will create more event types as they implement mechanics.
    /// </summary>

    /// <summary>
    /// Published when movement input is received.
    /// </summary>
    public readonly struct InputMoveEvent
    {
        public readonly Vector2 MoveInput;
        public readonly float Timestamp;

        public InputMoveEvent(Vector2 moveInput)
        {
            MoveInput = moveInput;
            Timestamp = Time.time;
        }
    }

    /// <summary>
    /// Published when jump button is pressed.
    /// </summary>
    public readonly struct InputJumpPressedEvent
    {
        public readonly float Timestamp;

        public InputJumpPressedEvent(float timestamp)
        {
            Timestamp = timestamp;
        }
    }

    /// <summary>
    /// Published when jump button is released.
    /// </summary>
    public readonly struct InputJumpReleasedEvent
    {
        public readonly float Timestamp;
        public readonly float HoldDuration;

        public InputJumpReleasedEvent(float timestamp, float holdDuration)
        {
            Timestamp = timestamp;
            HoldDuration = holdDuration;
        }
    }

    /// <summary>
    /// Published when dash button is pressed.
    /// </summary>
    public readonly struct InputDashPressedEvent
    {
        public readonly float Timestamp;

        public InputDashPressedEvent(float timestamp)
        {
            Timestamp = timestamp;
        }
    }
}
