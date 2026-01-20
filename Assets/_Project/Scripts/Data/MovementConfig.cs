using UnityEngine;

namespace PrecisionPlatformer.Data
{
    /// <summary>
    /// Configuration for player movement parameters.
    /// Students will create an instance of this in the Configs/Movement folder
    /// and tune these values for responsive platformer feel.
    ///
    /// SESSION 1: Students implement this config and tune acceleration/speed values.
    /// </summary>
    [CreateAssetMenu(fileName = "MovementConfig", menuName = "Platformer/Config/Movement Config", order = 1)]
    public class MovementConfig : ConfigBase
    {
        [Header("Horizontal Movement")]
        [Tooltip("Maximum horizontal movement speed (units per second)")]
        [Min(0.1f)]
        public float maxSpeed = 8f;

        [Tooltip("How quickly the player accelerates to max speed")]
        [Min(0.1f)]
        public float acceleration = 40f;

        [Tooltip("How quickly the player decelerates when not inputting movement")]
        [Min(0.1f)]
        public float deceleration = 50f;

        [Tooltip("Friction coefficient when grounded (multiplied with deceleration)")]
        [Range(0f, 1f)]
        public float frictionCoefficient = 0.95f;

        [Header("Advanced Tuning")]
        [Tooltip("Acceleration curve for more control over acceleration feel (optional)")]
        public AnimationCurve accelerationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Tooltip("Multiplier applied when changing direction mid-movement (snap turn)")]
        [Min(1f)]
        public float turnAroundBoost = 1.5f;

        [Header("Air Movement")]
        [Tooltip("Horizontal control multiplier when in air (1.0 = same as grounded)")]
        [Range(0f, 1f)]
        public float airControlMultiplier = 0.8f;

        public override bool Validate()
        {
            bool isValid = true;

            if (maxSpeed <= 0f)
            {
                Debug.LogWarning("MovementConfig: maxSpeed must be greater than 0");
                isValid = false;
            }

            if (acceleration <= 0f)
            {
                Debug.LogWarning("MovementConfig: acceleration must be greater than 0");
                isValid = false;
            }

            return isValid;
        }
    }
}
