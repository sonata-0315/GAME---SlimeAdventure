using UnityEngine;
using UnityEngine.InputSystem;
using PrecisionPlatformer.Core;
using PrecisionPlatformer.Events;

namespace PrecisionPlatformer.Input
{
    /// <summary>
    /// Input Handler that reads from Unity's Input System and publishes events to EventBus.
    /// This is a scaffold implementation - students will extend this with input buffering in Session 4.
    ///
    /// STUDENTS:
    /// - Session 1: Implement basic input reading and event publishing
    /// - Session 4: Add input buffering system for jump/dash
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        [Header("Input Action Asset")]
        [Tooltip("Reference to the InputSystem_Actions asset")]
        [SerializeField] private InputActionAsset inputActions;

        [Header("Dead Zone Settings")]
        [Tooltip("Minimum analog stick value to register as input")]
        [SerializeField] private float moveDeadZone = 0.1f;

        // Input action references
        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction dashAction;

        // Jump button tracking
        private float jumpPressTime = -1f;

        private void Awake()
        {
            // Get input action references
            if (inputActions != null)
            {
                moveAction = inputActions.FindActionMap("Player").FindAction("Move");
                jumpAction = inputActions.FindActionMap("Player").FindAction("Jump");
                dashAction = inputActions.FindActionMap("Player").FindAction("Sprint");
            }
            else
            {
                Debug.LogError("InputHandler: InputActionAsset not assigned!");
            }
        }

        private void OnEnable()
        {
            // Enable input actions
            moveAction?.Enable();
            jumpAction?.Enable();
            dashAction?.Enable();

            // Subscribe to button events
            if (jumpAction != null)
            {
                jumpAction.performed += OnJumpPerformed;
                jumpAction.canceled += OnJumpCanceled;
            }

            if (dashAction != null)
            {
                dashAction.performed += OnDashPerformed;
            }
        }

        private void OnDisable()
        {
            // Unsubscribe from button events
            if (jumpAction != null)
            {
                jumpAction.performed -= OnJumpPerformed;
                jumpAction.canceled -= OnJumpCanceled;
            }

            if (dashAction != null)
            {
                dashAction.performed -= OnDashPerformed;
            }

            // Disable input actions
            moveAction?.Disable();
            jumpAction?.Disable();
            dashAction?.Disable();
        }

        private void Update()
        {
            // Read movement input and publish event
            if (moveAction != null)
            {
                Vector2 moveInput = moveAction.ReadValue<Vector2>();

                // Apply dead zone
                if (moveInput.magnitude < moveDeadZone)
                {
                    moveInput = Vector2.zero;
                }

                // Publish movement event
                EventBus.Publish(new InputMoveEvent(moveInput));
            }
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            jumpPressTime = Time.time;
            EventBus.Publish(new InputJumpPressedEvent(jumpPressTime));
        }

        private void OnJumpCanceled(InputAction.CallbackContext context)
        {
            float holdDuration = jumpPressTime >= 0 ? Time.time - jumpPressTime : 0f;
            EventBus.Publish(new InputJumpReleasedEvent(Time.time, holdDuration));
            jumpPressTime = -1f;
        }

        private void OnDashPerformed(InputAction.CallbackContext context)
        {
            EventBus.Publish(new InputDashPressedEvent(Time.time));
        }

        // PUBLIC API for students to reference

        /// <summary>
        /// Get current movement input value (after dead zone applied).
        /// </summary>
        public Vector2 GetMoveInput()
        {
            if (moveAction == null) return Vector2.zero;

            Vector2 input = moveAction.ReadValue<Vector2>();
            return input.magnitude < moveDeadZone ? Vector2.zero : input;
        }

        /// <summary>
        /// Check if jump button is currently held.
        /// </summary>
        public bool IsJumpHeld()
        {
            return jumpAction != null && jumpAction.IsPressed();
        }

        /// <summary>
        /// Check if dash button is currently held.
        /// </summary>
        public bool IsDashHeld()
        {
            return dashAction != null && dashAction.IsPressed();
        }
    }
}
