using UnityEngine;

namespace Platformer.Config
{
    [CreateAssetMenu(menuName = "Config/Dash Config", fileName = "NewDashConfig")]
    public class DashConfig : ScriptableObject
    {
        [Header("Dash Physics")]
        [Tooltip("The explosive force applied when dashing")]
        public float DashSpeed = 25f;

        [Tooltip("How long the dash force is applied (in second).")]
        public float DashDuration = 0.2f;

        [Tooltip("How much velocity is kept after dash ends (0 = full stop, 1 = keep all mometum).")]
        public float PostDashDamping = 0.5f;

        [Header("Constraints")]
        [Tooltip("Time before you can dash again")]
        public float Cooldown = 1.0f;

        [Tooltip("Can dash in air?")]
        public bool AllowAirDash = true;

        [Header("Slime Feel (Juice)")]
        [Tooltip("Slightly squashes the sprite during dash.")]
        public Vector2 DashSquashScale = new Vector2(1.3f, 0.7f);

        [Header("Visual Effect")]
        [Tooltip("Particle effect prefab spawned when dash starts.")]
        public GameObject DashEffect;
        public Vector3 DashEffectOffset;
    }
}
