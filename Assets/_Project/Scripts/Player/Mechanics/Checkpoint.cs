using UnityEngine;
using Platformer.Mechanics;

namespace Platformer.GamePlay
{
    public class Checkpoint : MonoBehaviour
    {
        [Header("Checkpoint Sprites")]
        [Tooltip("The sprite to display when this checkpoint is activated.")]
        [SerializeField] private Sprite _activeSprite;

        [Header("Juice")]
        [SerializeField] private GameObject _activationEffect;

        private bool _isActivated = false;
        private SpriteRenderer _renderer;

        private void Start()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isActivated) return;

            if (other.CompareTag("Player"))
            {
                PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();

                if (respawn != null)
                {
                    respawn.SetRespawnPoint(transform.position);
                    ActivateCheckpoint();
                }
            }
        }

        private void ActivateCheckpoint()
        {
            _isActivated = true;

            if (_renderer != null &&  _activeSprite != null)
            {
                _renderer.sprite = _activeSprite;
            }

            if (_activationEffect != null)
            {
                Instantiate(_activationEffect, transform.position, Quaternion.identity);
            }

            Debug.Log("Checkpint saved.");
        }
    }
}