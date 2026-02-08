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
        public float SpeedChangeRate = 10.0f;
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;

        public bool canMove = false;

        [Header("Player Grounded")]
        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.28f;
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        public GameObject CinemachineCameraTarget;
        public bool LockCameraPosition = false;

        private float _speed;
        private float _animationBlend;
        private float _verticalVelocity;

        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private Animator _animator;
#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private bool _hasAnimator;

        private Camera _mainCamera;

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

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
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            GroundedCheck();
            JumpAndGravity();

            if (canMove)
                Move();
        }

        private void Move()
        {
            // Determine target speed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
            if (_input.move.x == 0) targetSpeed = 0f;

            // Smooth speed change
            float currentHorizontalSpeed = Mathf.Abs(_controller.velocity.x);
            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? Mathf.Abs(_input.move.x) : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
            else
                _speed = targetSpeed;

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

            // Flip sprite based on horizontal input
            if (_input.move.x != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = _input.move.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                transform.localScale = scale;
            }

            // Move player strictly on X-axis
            Vector3 targetDirection = Vector3.right * _input.move.x;
            _controller.Move(targetDirection * (_speed * Time.deltaTime) + new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);

            // Update animator
            if (_hasAnimator)
                _animator.SetFloat("Speed", _animationBlend);
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

            if (_hasAnimator)
                _animator.SetBool("Grounded", Grounded);
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = 0.15f;

                if (_verticalVelocity < 0f)
                    _verticalVelocity = -2f;

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

            if (_verticalVelocity < 53f)
                _verticalVelocity += Gravity * Time.deltaTime;
        }

        /// <summary>
        /// Teleport / spawn player flat for 2D side-view.
        /// </summary>
        /// <param name="position">Spawn position</param>
        /// <param name="facingRight">True if facing right</param>
        public void ResetOrientation(Vector3 position, bool facingRight)
        {
            transform.position = position;

            // Keep flat on X-axis for side view
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            // Flip sprite
            Vector3 scale = transform.localScale;
            scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;

            // Lock camera briefly
            LockCameraPosition = true;
            Invoke(nameof(UnlockCamera), 0.1f);
        }

        private void UnlockCamera() => LockCameraPosition = false;
    }
}