using UnityEngine;

namespace Platformer.Config
{
    [CreateAssetMenu(menuName = "Config/Wall Config", fileName = "NewWallConfig")]
    public class wallMechanicConfig : ScriptableObject
    {
        [Header("Wall Slide (Slime Stickiness)")]
        [Tooltip("How fast the slime slides down the wall. Lower = stickier.")]
        public float SlideSpeed = 3f;

        [Tooltip("If true, you only slide when pressing INTO the wall.")]
        public bool RequireInputToSlide = false;

        [Header("Wall Jump")]
        [Tooltip("Total explosive force of the wall jump.")]
        public float WallJumpForce = 25f;

        [Tooltip("The angle of the jump. (X = Away from wall, Y = Up). normalized automatically.")]
        public Vector2 WallJumpAngle = new Vector2(1.5f, 2f);

        [Header("Control Feel")]
        [Tooltip("How long (seconds) to ignore horizontal input after a wall jump. Keeps player from immediately drifiting back to the wall.")]
        public float WallJumpInputFreezeTimer = 0.2f;

        [Header("Stamina System")]
        [Tooltip("Max stamina (e.g. 100).")]
        public float MaxStamina = 100f;

        [Tooltip("How much stamina is consumed per wall jump.")]
        public float WallJumpCost = 30f;

        [Tooltip("How fast stamina regenerates per second.")]
        public float StaminaRegenRate = 15f;

        [Header("Visual Effect")]
        [Tooltip("Particle effect spawned when jumping off the wall.")]
        public GameObject WallJumpEffect;
        public float EffectSpawnOffset = 0.5f; //partivle effect spawn how far from the wall
    }
}
