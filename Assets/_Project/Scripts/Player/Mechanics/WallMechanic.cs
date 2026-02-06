using UnityEngine;
using System.Collections;
using Platformer.Config;
using Platformer.Core;
using Platformer.Player;

namespace Platformer.Mechanics
{
    public class WallMechanic : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private wallMechanicConfig _config;

        [Header("References")]
        [SerializeField] private Transform _visualRoot;

        private Rigidbody2D _rb;
        private PlayerController _controller;
        private InputReader _input;

        private bool _isWallSliding;
        private float _wallJumpLockTimer;
        private int _wallDir;
        private Vector3 _orginalScale;
        private float _currentStamina;
        public bool IsWallSliding => _isWallSliding;

        private void Start()
        {
            _controller = GetComponent<PlayerController>();
            _rb = GetComponent<Rigidbody2D>();
            _input = ServiceLocator.Get<InputReader>();

            if (_visualRoot == null && transform.childCount > 0)
                _visualRoot = transform.GetChild(0);

            if (_visualRoot != null)
                _orginalScale = _visualRoot.localScale;

            _currentStamina = _config.MaxStamina;
        }

        private void Update()
        {
            HandleStaminaRegen();

            if (_wallJumpLockTimer > 0)
            {
                _wallJumpLockTimer -= Time.deltaTime;
            }

            if (_input.JumpBuffered && _isWallSliding)
            {
                if (_currentStamina >= _config.WallJumpCost)
                {
                    PerformWallJump();
                    _input.ConsumeJumpBuffer();
                }
                else
                {
                    Debug.Log("No Stamina!");
                    _input.ConsumeJumpBuffer();
                }
            }
        }

        private void FixedUpdate()
        {
            CheckWall();
            HandleWallSlide();
        }

        private void CheckWall()
        {
            float checkDist = 1.5f;

            LayerMask wallLayer = _controller.Config.groundLayer;

            bool isTouchinRight = Physics2D.Raycast(transform.position, Vector2.right, checkDist, wallLayer);
            bool isTouchinLeft = Physics2D.Raycast(transform.position, Vector2.left, checkDist, wallLayer);

            if (isTouchinRight) _wallDir = 1;
            else if (isTouchinLeft) _wallDir = -1;
            else _wallDir = 0;

            bool isFalling = _rb.linearVelocity.y < 0;

            bool touchingWall = _wallDir != 0;

            _isWallSliding = touchingWall && !_controller.IsGrounded && isFalling;
        }

        private void HandleStaminaRegen()
        {
            if (_controller.IsGrounded)
            {
                _currentStamina = _config.MaxStamina;
            }
            else
            {
                _currentStamina += _config.StaminaRegenRate * Time.deltaTime;
                _currentStamina = Mathf.Min(_currentStamina, _config.MaxStamina);
            }
        }

        private void HandleWallSlide()
        {
            if (_isWallSliding)
            {
                float currentY = _rb.linearVelocity.y;
                if (currentY < -_config.SlideSpeed)
                {
                    _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, -_config.SlideSpeed);
                }

                if (_visualRoot != null)
                {
                    _visualRoot.localScale = new Vector3(_orginalScale.x * 0.9f, _orginalScale.y * 1.1f, 1f);
                }
            }
            else if (!_input.DashHeld)
            {
                if (_visualRoot != null && Vector3.Distance(_visualRoot.localScale, _orginalScale) > 0.01f)
                {
                    _visualRoot.localScale = Vector3.Lerp(_visualRoot.localScale, _orginalScale, 10 * Time.fixedDeltaTime);
                }
            }
        }

        private void PerformWallJump()
        {
            _currentStamina -= _config.WallJumpCost;

            Vector2 jumpDir = new Vector2(-_wallDir * _config.WallJumpAngle.x, _config.WallJumpAngle.y).normalized;

            if (_config.WallJumpEffect != null)
            {
                Vector3 spawnPos = transform.position + (Vector3.right * _wallDir * _config.EffectSpawnOffset);
                Quaternion rotation = Quaternion.LookRotation(new Vector3(-_wallDir, 1, 0));
                Instantiate(_config.WallJumpEffect, spawnPos, rotation);
            }

            _rb.linearVelocity = jumpDir * _config.WallJumpForce;

            _wallJumpLockTimer = _config.WallJumpInputFreezeTimer;
        }

        public bool ShouldBlockMoveInput()
        {
            return _wallJumpLockTimer > 0;
        }
    }
}
