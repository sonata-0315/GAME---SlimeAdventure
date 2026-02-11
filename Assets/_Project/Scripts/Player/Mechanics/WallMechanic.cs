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
        private Collider2D _collider;
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
            _collider = GetComponent<Collider2D>();
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

            //when sliding press jump button counts wall jump
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
                    //makes the movement fluent
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
            //auto caculate the distance from player to the wall
            float checkDist = _collider.bounds.extents.x + 0.2f;

            LayerMask wallLayer = _controller.Config.groundLayer;

            //Raycast test left and right
            bool isTouchinRight = Physics2D.Raycast(transform.position, Vector2.right, checkDist, wallLayer);
            bool isTouchinLeft = Physics2D.Raycast(transform.position, Vector2.left, checkDist, wallLayer);

            //confirmed wall direction
            if (isTouchinRight) _wallDir = 1;
            else if (isTouchinLeft) _wallDir = -1;
            else _wallDir = 0;

            //check if it's sliding
            bool isFalling = _rb.linearVelocity.y < 0;

            bool touchingWall = _wallDir != 0;

            //only whern touch wall + not grounded + is falling count as wall sliding
            //only trigger when it falls down
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
                //limit the falling speed
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

            //caculate the jumping direction
            Vector2 jumpDir = new Vector2(-_wallDir * _config.WallJumpAngle.x, _config.WallJumpAngle.y).normalized;

            if (_config.WallJumpEffect != null)
            {
                Vector3 spawnPos = transform.position + (Vector3.right * _wallDir * _config.EffectSpawnOffset);
                Quaternion rotation = Quaternion.LookRotation(new Vector3(-_wallDir, 1, 0));
                Instantiate(_config.WallJumpEffect, spawnPos, rotation);
            }
            //add force
            _rb.linearVelocity = jumpDir * _config.WallJumpForce;
            //lock input time, incase player go back to the wall immdiately
            _wallJumpLockTimer = _config.WallJumpInputFreezeTimer;
        }

        public bool ShouldBlockMoveInput()
        {
            return _wallJumpLockTimer > 0;
        }
    }
}
