using System.Collections;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class DisappearingPlatform : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Time before the platform disappears.")]
        public float DisappearDelay = 3.0f;

        [Tooltip("Time before the platform respawns (0 = never respawn).")]
        public float RespawnDelay = 5.0f;

        [Header("Juice (Shake)")]
        [Tooltip("How violently it shakes before disappearing")]
        public float ShakeAmount = 0.05f;

        private Collider2D _collider;
        private SpriteRenderer _renderer;
        private Vector3 _orginalPos;
        private bool _isTriggered = false;

        private void Start()
        {
            
            _collider = GetComponent<Collider2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _orginalPos = transform.position;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isTriggered) return;

            if (collision.gameObject.CompareTag("Player"))
            {
                //check if player is on the top
                foreach (ContactPoint2D contact in collision.contacts)
                {
                    if (contact.normal.y < -0.5f)
                    {
                        StartCoroutine(CrumbleRoutine());
                        break;
                    }
                }
            }
        }

        private IEnumerator CrumbleRoutine()
        {
            _isTriggered = true;
            float timer = 0f;
            while (timer < DisappearDelay)
            {
                timer += Time.deltaTime;

                //shake effect
                float currentShake = ShakeAmount * (timer / DisappearDelay);
                float x = (Random.value - 0.5f) * currentShake;
                float y = (Random.value - 0.5f) * currentShake;

                transform.position = _orginalPos + new Vector3(x, y, 0);

                yield return null;
            }

            //disapear
            transform.position = _orginalPos;

            if (_renderer) _renderer.enabled = false;
            if (_collider) _collider.enabled = false;

            if (RespawnDelay > 0)
            {
                yield return new WaitForSeconds(RespawnDelay);

                if (_renderer) _renderer.enabled = true;
                if (_collider) _collider.enabled = true;
                _isTriggered = false;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
