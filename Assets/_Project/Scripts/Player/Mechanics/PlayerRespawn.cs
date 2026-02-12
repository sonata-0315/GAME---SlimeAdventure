using UnityEngine;
using Platformer.Core;
using Platformer.Player;

namespace Platformer.Mechanics
{
    public class PlayerRespawn : MonoBehaviour
    {
        [Header("Juice")]
        [SerializeField] private GameObject _deathEffect;

        private Vector3 _respawnPosition;
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            //when game start, record this location as respawn position
            _respawnPosition = transform.position;
        }

        public void Die()
        {
            StartCoroutine(RespawnRoutine());
        }
        private System.Collections.IEnumerator RespawnRoutine()
        {
            if (_deathEffect != null) Instantiate(_deathEffect, transform.position, Quaternion.identity);

            var renderer = GetComponentInChildren<SpriteRenderer>();
            var col = GetComponent<Collider2D>();
            var controller = GetComponent<PlayerController>();

            controller.enabled = false;
            renderer.enabled = false;
            col.enabled = false;
            _rb.simulated = false;

            yield return new WaitForSeconds(0.5f);

            transform.position = _respawnPosition;
            _rb.linearVelocity = Vector3.zero;

            controller.enabled = true;
            renderer.enabled = true;
            col.enabled = true;
            _rb.simulated = true;
        }

        public void SetRespawnPoint(Vector3 newPoint)
        {
            _respawnPosition = newPoint;
        }
    }
}