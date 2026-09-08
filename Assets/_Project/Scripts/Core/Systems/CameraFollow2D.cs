using UnityEngine;

namespace Core.Systems
{
    public sealed class CameraFollow2D : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform _target;

        [Header("Follow Settings")]
        [SerializeField] private Vector3 _offset = new Vector3(0f, 0.5f, -10f);
        [SerializeField] private float _smoothSpeed = 5f;

        [Header("Look Ahead")]
        [SerializeField] private float _lookAheadDistance = 1.2f;
        [SerializeField] private float _lookAheadSpeed = 3f;

        private float _currentLookAheadX;
        private float _targetLookAheadX;

        private void LateUpdate()
        {
            if (_target == null) return;

            float moveDirection = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(moveDirection) > 0.01f)
            {
                _targetLookAheadX = moveDirection * _lookAheadDistance;
            }

            _currentLookAheadX = Mathf.Lerp(_currentLookAheadX, _targetLookAheadX, _lookAheadSpeed * Time.deltaTime);

            Vector3 desiredPosition = _target.position + _offset;
            desiredPosition.x += _currentLookAheadX;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
        }
    }
}