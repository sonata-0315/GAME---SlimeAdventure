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

        //Dependence
        private Rigidbody2D _rb;
        private PlayerController _controller;
        private InputReader _input;

        private bool _isDashing;
        private float _lastDashTime;
        private Vector3 _originalScale;

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
            if (_input.DashBuffered) //Debug.Log("Dash prepared!")

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
            //no gravity, make sure dash in straight line
            float originalGrav = _rb.gravityScale;
            _rb.gravityScale = 0;

            //decide dash direction
            Vector2 dashDir = _input.MoveInput;
            if (dashDir == Vector2.zero)
            {
                // if direction key is correct, dash to horizontal direction
                float facing = _rb.linearVelocity.x >= 0 ? 1 : -1;
                dashDir = new Vector2(facing, 0);
            }
            dashDir = dashDir.normalized;

            if (_visualRoot != null)
            {
                Vector3 targetScale = new Vector3(
                    _originalScale.x * _config.DashSquashScale.x,
                    _originalScale.y * _config.DashSquashScale.y,
                    1f
                    );
            }
            //this will cover up the playercontroller speed
            _rb.linearVelocity = dashDir * _config.DashSpeed;

            yield return new WaitForSeconds(_config.DashDuration);

            if (_visualRoot != null)
            {
                _visualRoot.localScale = _originalScale;
            }

            _rb.gravityScale = originalGrav; //gravity back!
            _rb.linearVelocity *= _config.PostDashDamping; //slow down
            _isDashing = false;
        }

    }
}
