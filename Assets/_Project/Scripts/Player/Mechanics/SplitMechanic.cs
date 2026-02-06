using UnityEngine;
using Platformer.Config;
using Platformer.Player;
using Platformer.Core;
using Unity.VisualScripting;

namespace Platformer.Mechanics
{
    public class SplitMechanic : MonoBehaviour
    {
        [SerializeField] private SplitConfig _config;

        private Animator _playerAnimator;

        private bool _isSplit = false;
        public bool IsSplit => _isSplit;
        private GameObject _activeClone;
        private Vector3 _originalScale;
        private InputReader _input;

        private void Start()
        {
            _originalScale = transform.localScale;

            _input = ServiceLocator.Get<InputReader>();
            if (_input != null)
            {
                _input.SplitEvent += HandleSplitAction;
            }

            _playerAnimator = GetComponent<Animator>();
        }

        public void HandleSplitAction()
        {
            if (_isSplit)
            {
                Merge();
            }
            else
            {
                Split();
            }
        }

        private void Split()
        {
            if (_config.ClonePrefab == null) return;

            float facingDir = transform.localScale.x > 0 ? 1 : -1;
            Vector3 spawnPos = transform.position + new Vector3(_config.SpawnOffset.x * facingDir, _config.SpawnOffset.y, 0);

            _activeClone = Instantiate(_config.ClonePrefab, spawnPos, Quaternion.identity);

            Animator cloneAnimator = _activeClone.GetComponent<Animator>();

            if (cloneAnimator != null && _playerAnimator != null)
            {
                cloneAnimator.runtimeAnimatorController = _playerAnimator.runtimeAnimatorController;

                _activeClone.transform.localScale = _originalScale;
            } 

            var rb = _activeClone.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.mass = _config.CloneMass;
                rb.linearDamping = _config.CloneDrag;

                Vector2 force = new Vector2(_config.SpawnForce.x * facingDir, _config.SpawnForce.y);
                rb.AddForce(force, ForceMode2D.Impulse);
            }

            float newX = Mathf.Sign(transform.localScale.x) * Mathf.Abs(_originalScale.x * _config.PlayerShrinkScale);
            float newY = _originalScale.y * _config.PlayerShrinkScale;
            transform.localScale = new Vector3(newX, newY, _originalScale.z);

            _isSplit = true;
        }

        private void Merge()
        {
            if (_activeClone != null)
            {
                Destroy(_activeClone);
            }

            float currentFacing = Mathf.Sign(transform.localScale.x);
            transform.localScale = new Vector3(_originalScale.x * currentFacing, _originalScale.y * _originalScale.z);

            _isSplit = false;
        }

        private void OnDestroy()
        {
            if (_input != null) _input.SplitEvent -= HandleSplitAction;
        }
    }
}
