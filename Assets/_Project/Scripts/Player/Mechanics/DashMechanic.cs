using UnityEngine;
using System.Collections;
using Platformer.Config;
using Platformer.Core;
using Platformer.Player;
using UnityEngine.InputSystem.Interactions;

namespace Platformer.Mechanics
{
    public class DashMechanic : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private DashConfig _config;

        [Header("References")]
        [SerializeField] private Transform _visualRoot;

        private Rigidbody2D _rb;
        private PlayerController _controller;
        private InputReader _input;

        private bool _isDashing;
        private float _lastDashTime;
        private Vector3 _originalScale;

        private float _lastFacingDirection = 1f;

        public bool IsDashing => _isDashing;

        private void Start()
        {
            _controller = GetComponent<PlayerController>();
            _rb = GetComponent<Rigidbody2D>();
            _input = ServiceLocator.Get<InputReader>();

            if (_visualRoot == null && transform.childCount > 0)
                _visualRoot = transform.GetChild(0);

            if (_visualRoot != null)
                _originalScale = _visualRoot.localScale;
        }

        private void Update()
        {
            // Update facing direction constantly based on Input
            if (_input.MoveInput.x != 0)
            {
                _lastFacingDirection = Mathf.Sign(_input.MoveInput.x);
            }
            

            if (_input.DashBuffered && CanDash())
            {
                _input.ConsumeDashBuffer();
                StartCoroutine(PerformDash());
            }
        }

        private bool CanDash()
        {
            if (Time.time < _lastDashTime + _config.Cooldown) return false;
            if (_isDashing) return false;
            if (!_controller.IsGrounded && !_config.AllowAirDash) return false;
            return true;
        }

        private IEnumerator PerformDash()
        {
            _isDashing = true;
            _lastDashTime = Time.time;

            if (_config.DashEffect != null)
            {
                Instantiate(_config.DashEffect, transform.position + _config.DashEffectOffset, Quaternion.identity);
            }

            float originalGrav = _rb.gravityScale;
            _rb.gravityScale = 0;

            

            
            Vector2 rawInput = _input.MoveInput;

            // Handle "No Input" case -> Dash forward
            if (rawInput == Vector2.zero)
            {
                rawInput = new Vector2(_lastFacingDirection, 0);
            }

            Vector2 dashDir = rawInput.normalized;

            // 2. Clamp the Y component
            // If the Y component exceeds our limit (e.g., 0.71), we cap it.
            if (Mathf.Abs(dashDir.y) > _config.Angle)
            {
                float signY = Mathf.Sign(dashDir.y);
                float newY = _config.Angle * signY;

                
                float newX = Mathf.Sqrt(1 - (newY * newY));

                // Determine X direction:
                // If the player pressed ANY X direction, use that sign.
                // If the player pressed PURE UP/DOWN (x=0), use the last facing direction.
                float signX = (Mathf.Abs(dashDir.x) > 0.01f) ? Mathf.Sign(dashDir.x) : _lastFacingDirection;

                dashDir = new Vector2(newX * signX, newY);
            }

            

            if (_visualRoot != null)
            {
                _visualRoot.localScale = new Vector3(
                    _originalScale.x * _config.DashSquashScale.x,
                    _originalScale.y * _config.DashSquashScale.y,
                    1f
                );
            }

            _rb.linearVelocity = dashDir * _config.DashSpeed;

            yield return new WaitForSeconds(_config.DashDuration);

            if (_visualRoot != null)
            {
                _visualRoot.localScale = _originalScale;
            }

            _rb.gravityScale = originalGrav;
            _rb.linearVelocity *= _config.PostDashDamping;
            _isDashing = false;
        }
    }
}
