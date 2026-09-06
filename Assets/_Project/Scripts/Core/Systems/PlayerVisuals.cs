using UnityEngine;

namespace Core.Systems.Movement
{
    [RequireComponent(typeof(Animator), typeof(PlayerMotor2D))]
    public class PlayerVisuals : MonoBehaviour
    {
        private Animator _animator;
        private PlayerMotor2D _motor;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _motor = GetComponent<PlayerMotor2D>();
        }

        private void Update()
        {
            if (_motor == null || _animator == null) return;

            // Enviamos la velocidad horizontal en valor absoluto
            _animator.SetFloat(SpeedHash, _motor.HorizontalVelocity);
            _animator.SetBool(IsGroundedHash, _motor.IsGrounded);
        }
    }
}