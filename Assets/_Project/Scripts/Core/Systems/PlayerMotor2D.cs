using System.Collections;
using UnityEngine;

namespace Core.Systems.Movement
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public sealed class PlayerMotor2D : MonoBehaviour
    {
        [Header("Horizontal Movement")]
        [SerializeField] private float _moveSpeed = 9f;
        [SerializeField] private float _acceleration = 75f;
        [SerializeField] private float _deceleration = 65f;

        [Header("Jump Tuning")]
        [SerializeField] private float _jumpForce = 13.5f;
        [SerializeField] private float _fallMultiplier = 3.2f;
        [SerializeField] private float _lowJumpMultiplier = 2.2f;
        [SerializeField] private float _coyoteTimeDuration = 0.12f;
        [SerializeField] private float _jumpBufferDuration = 0.12f;
        [SerializeField] private int _maxAirJumps = 1;

        [Header("Dash Tuning")]
        [SerializeField] private float _dashSpeed = 22f;
        [SerializeField] private float _dashDuration = 0.18f;
        [SerializeField] private float _dashCooldown = 0.8f;

        [Header("Ground Collision Detection")]
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.75f, 0.1f);
        [SerializeField] private LayerMask _groundLayer;

        private Rigidbody2D _rb;
        private float _horizontalInput;
        private bool _isJumpHeld;
        private float _coyoteTimer;
        private float _jumpBufferTimer;
        private int _airJumpsRemaining;
        private bool _isGrounded;
        private bool _isDashing;
        private bool _canDash = true;
        private float _facingDirection = 1f;

        public bool IsDashing => _isDashing;
        public bool IsGrounded => _isGrounded;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void SetMovementInput(float horizontalInput)
        {
            _horizontalInput = horizontalInput;
            if (Mathf.Abs(horizontalInput) > 0.05f)
            {
                _facingDirection = Mathf.Sign(horizontalInput);
            }
        }

        public void SetJumpHeld(bool isHeld) => _isJumpHeld = isHeld;

        public void RegisterJumpInput() => _jumpBufferTimer = _jumpBufferDuration;

        public void TriggerDash()
        {
            if (_canDash && !_isDashing)
            {
                StartCoroutine(PerformDashRoutine());
            }
        }

        private void Update()
        {
            if (_jumpBufferTimer > 0f) _jumpBufferTimer -= Time.deltaTime;

            if (_isGrounded)
            {
                _coyoteTimer = _coyoteTimeDuration;
                _airJumpsRemaining = _maxAirJumps;
            }
            else if (_coyoteTimer > 0f)
            {
                _coyoteTimer -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            CheckGroundStatus();

            if (_isDashing) return;

            ApplyHorizontalMovement();
            ProcessJumpExecution();
            ApplyVariableGravity();
        }

        private void CheckGroundStatus()
        {
            _isGrounded = Physics2D.OverlapBox(
                _groundCheckPoint.position,
                _groundCheckSize,
                0f,
                _groundLayer
            );
        }

        private void ApplyHorizontalMovement()
        {
            float targetSpeed = _horizontalInput * _moveSpeed;
            float speedDifference = targetSpeed - _rb.linearVelocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _acceleration : _deceleration;
            float movementForce = speedDifference * accelRate;

            _rb.AddForce(movementForce * Vector2.right, ForceMode2D.Force);
        }

        private void ProcessJumpExecution()
        {
            if (_jumpBufferTimer <= 0f) return;

            if (_coyoteTimer > 0f)
            {
                ExecuteJumpImpulse();
                _coyoteTimer = 0f;
                _jumpBufferTimer = 0f;
            }
            else if (_airJumpsRemaining > 0)
            {
                ExecuteJumpImpulse();
                _airJumpsRemaining--;
                _jumpBufferTimer = 0f;
            }
        }

        private void ExecuteJumpImpulse()
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0f);
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        private void ApplyVariableGravity()
        {
            if (_rb.linearVelocity.y < 0f)
            {
                _rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (_fallMultiplier - 1f) * Time.fixedDeltaTime);
            }
            else if (_rb.linearVelocity.y > 0f && !_isJumpHeld)
            {
                _rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (_lowJumpMultiplier - 1f) * Time.fixedDeltaTime);
            }
        }

        private IEnumerator PerformDashRoutine()
        {
            _canDash = false;
            _isDashing = true;

            float originalGravity = _rb.gravityScale;
            _rb.gravityScale = 0f;
            _rb.linearVelocity = new Vector2(_facingDirection * _dashSpeed, 0f);

            yield return new WaitForSeconds(_dashDuration);

            _rb.gravityScale = originalGravity;
            _isDashing = false;

            yield return new WaitForSeconds(_dashCooldown);
            _canDash = true;
        }

        private void OnDrawGizmosSelected()
        {
            if (_groundCheckPoint == null) return;
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        }
    }
}
