using UnityEngine;

namespace Platformer.Config
{
    [CreateAssetMenu(menuName = "Config/Split Config", fileName = "NewSplitConfig")]
    public class SplitConfig : ScriptableObject
    {
        [Header("Clone Settings")]
        [Tooltip("The prefab to spawn as the clone.")]
        public GameObject ClonePrefab;

        [Tooltip("How much force to throw the clone out.")]
        public Vector2 SpawnForce = new Vector2(5f, 5f);

        [Tooltip("Offset position to spawn the clone (relative to player)")]
        public Vector2 SpawnOffset = new Vector2(1f, 0f);

        [Header("Player Changes")]
        [Tooltip("Scale multiplier for the player when split (e.g., 0.7 for 70% size).")]
        public float PlayerShrinkScale = 0.7f;

        [Header("Lone Physics")]
        [Tooltip("Mass of the clone (higher = harder to push).")]
        public float CloneMass = 100f;

        [Tooltip("Linear Drag (higher = stops sliding faster).")]
        public float CloneDrag = 5f;
    }
}
