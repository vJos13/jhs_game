using UnityEngine;

namespace Core.Systems.Movement
{
    [RequireComponent(typeof(PlayerMotor2D))]
    public sealed class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private KeyCode _dashKey = KeyCode.LeftShift;

        private PlayerMotor2D _motor;

        private void Awake()
        {
            _motor = GetComponent<PlayerMotor2D>();
        }

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            _motor.SetMovementInput(horizontal);

            if (Input.GetButtonDown("Jump"))
            {
                _motor.RegisterJumpInput();
            }

            _motor.SetJumpHeld(Input.GetButton("Jump"));

            if (Input.GetKeyDown(_dashKey))
            {
                _motor.TriggerDash();
            }
        }
    }
}