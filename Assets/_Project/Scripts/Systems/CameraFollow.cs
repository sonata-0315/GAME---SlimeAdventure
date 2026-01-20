using UnityEngine;

namespace PrecisionPlatformer.Systems
{
    /// <summary>
    /// Simple camera follow script for 2D platformer.
    /// Smoothly follows the player with configurable offset and smoothing.
    /// Provided as-is for students - they can modify if needed.
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target")]
        [Tooltip("Transform to follow (typically the player)")]
        [SerializeField] private Transform target;

        [Header("Follow Settings")]
        [Tooltip("Offset from target position")]
        [SerializeField] private Vector3 offset = new Vector3(0f, 2f, -10f);

        [Tooltip("Smooth follow speed (higher = faster, 0 = instant)")]
        [SerializeField] private float smoothSpeed = 5f;

        [Header("Bounds (Optional)")]
        [Tooltip("Enable camera bounds to prevent showing outside level")]
        [SerializeField] private bool useBounds = false;

        [Tooltip("Minimum X position")]
        [SerializeField] private float minX = -100f;

        [Tooltip("Maximum X position")]
        [SerializeField] private float maxX = 100f;

        [Tooltip("Minimum Y position")]
        [SerializeField] private float minY = -100f;

        [Tooltip("Maximum Y position")]
        [SerializeField] private float maxY = 100f;

        private void LateUpdate()
        {
            if (target == null)
            {
                // Try to find player if not assigned
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                    Debug.Log("CameraFollow: Found player target");
                }
                return;
            }

            // Calculate desired position
            Vector3 desiredPosition = target.position + offset;

            // Apply bounds if enabled
            if (useBounds)
            {
                desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
                desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
            }

            // Smooth follow
            if (smoothSpeed > 0f)
            {
                transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = desiredPosition;
            }
        }

        // Gizmo visualization for bounds
        private void OnDrawGizmosSelected()
        {
            if (useBounds)
            {
                Gizmos.color = Color.yellow;
                Vector3 center = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0f);
                Vector3 size = new Vector3(maxX - minX, maxY - minY, 0f);
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}
