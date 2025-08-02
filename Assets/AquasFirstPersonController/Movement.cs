using UnityEngine;
using UnityEngine.InputSystem;

namespace AquasAssets // Feel free to change the namespace :3
{
    public class Movement : MonoBehaviour
    {
        [Header("References")]
        public Transform cameraTransform;
        public CharacterController characterController;

        [Header("Settings")]
        public float moveSpeed = 5f;
        public float lookSensitivity = 0.5f;
        public float jumpHeight = 1.5f;
        public float gravity = -9.81f;

        [Header("Ground Check")]
        public Transform groundCheckPoint;
        public float groundCheckRadius = 0.2f;
        public LayerMask groundLayer;

        public InputAction moveAction;
        public InputAction lookAction;
        public InputAction jumpAction;

        private Vector2 lookDelta;
        private Vector2 moveInput;
        private float cameraPitch;

        private Vector3 velocity;
        private bool isGrounded;

        private void OnEnable()
        {
            moveAction.Enable();
            lookAction.Enable();
            jumpAction.Enable();
        }

        private void OnDisable()
        {
            moveAction.Disable();
            lookAction.Disable();
            jumpAction.Disable();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            moveInput = moveAction.ReadValue<Vector2>();
            lookDelta = lookAction.ReadValue<Vector2>() * lookSensitivity;

            Look();
            Move();
            HandleJumpAndGravity();
        }

        private void Move()
        {
            Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
            characterController.Move(move * moveSpeed * Time.deltaTime);
        }

        private void Look()
        {
            cameraPitch -= lookDelta.y;
            cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

            cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
            transform.Rotate(Vector3.up * lookDelta.x);
        }

        private void HandleJumpAndGravity()
        {
            // Use ground check instead of characterController.isGrounded
            isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // small downward force to stick to ground
            }

            if (jumpAction.triggered && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
    }

}

