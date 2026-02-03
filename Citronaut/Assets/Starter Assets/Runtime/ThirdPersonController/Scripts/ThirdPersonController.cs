using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        public float MoveSpeed = 2.0f;
        public float SprintSpeed = 5.335f;
        public float RotationSmoothTime = 0.12f;
        public float SpeedChangeRate = 10.0f;
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;

        public bool canMove = false; // false at start


        [Header("Player Grounded")]
        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.28f;
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        public GameObject CinemachineCameraTarget;
        public float TopClamp = 70.0f;
        public float BottomClamp = -30.0f;
        public float CameraAngleOverride = 0.0f;
        public bool LockCameraPosition = false;

        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private Animator _animator;
#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private bool _hasAnimator;
        private const float _threshold = 0.01f;

        private Camera _mainCamera;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#endif
            _animator = GetComponent<Animator>();
            _hasAnimator = _animator != null;

            _mainCamera = Camera.main;
        }

        private void Start()
        {
            _jumpTimeoutDelta = 0.5f;
            _fallTimeoutDelta = 0.15f;
            _targetRotation = transform.eulerAngles.y; // Lock rotation to current
            _rotationVelocity = 0f; // Reset any smooth-damp speed


        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Update camera reference when a new scene loads
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            GroundedCheck();
            JumpAndGravity();

            if (canMove)
            {
                Move();
            }
        }

        private void Move()
        {
            // 1. Only use horizontal input (A/D or Left Stick)
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
            if (_input.move.x == 0) targetSpeed = 0f;

            // Calculate speed based on X input only
            float currentHorizontalSpeed = Mathf.Abs(_controller.velocity.x);
            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? Mathf.Abs(_input.move.x) : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

            // 2. 2D Rotation Logic (Snap to 90 or -90 degrees)
            if (_input.move.x != 0)
            {
                // If moving right (x > 0), target rotation is 90. If left (x < 0), target is 270.
                _targetRotation = _input.move.x > 0 ? 90f : 270f;

                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
                transform.rotation = Quaternion.Euler(0f, rotation, 0f);
            }

            // 3. Apply Movement strictly on the X-axis
            // Vector3.right is (1, 0, 0). Multiplying by _input.move.x handles direction.
            Vector3 targetDirection = Vector3.right * (_input.move.x > 0 ? 1f : -1f);

            _controller.Move(targetDirection * (_speed * Time.deltaTime) +
                             new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);

            if (_hasAnimator)
            {
                _animator.SetFloat("Speed", _animationBlend);
            }
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

            if (_hasAnimator)
            {
                _animator.SetBool("Grounded", Grounded);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = 0.15f;

                if (_verticalVelocity < 0f)
                    _verticalVelocity = -2f;

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0f)
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    if (_hasAnimator)
                        _animator.SetBool("Jump", true);
                }

                if (_jumpTimeoutDelta >= 0f)
                    _jumpTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                _jumpTimeoutDelta = 0.5f;

                if (_fallTimeoutDelta >= 0f)
                    _fallTimeoutDelta -= Time.deltaTime;
                else if (_hasAnimator)
                    _animator.SetBool("FreeFall", true);

                _input.jump = false;
            }

            if (_verticalVelocity < _terminalVelocity)
                _verticalVelocity += Gravity * Time.deltaTime;
        }

        public void ResetOrientation(Transform spawn)
        {
            // Force player facing direction
            transform.rotation = spawn.rotation;

            // Sync movement rotation logic
            _targetRotation = spawn.eulerAngles.y;
            _rotationVelocity = 0f;

            // Prevent Cinemachine snap on first frame
            LockCameraPosition = true;

            // Release camera lock next frame
            Invoke(nameof(UnlockCamera), 0.1f);
        }

        private void UnlockCamera()
        {
            LockCameraPosition = false;
        }
    }
}