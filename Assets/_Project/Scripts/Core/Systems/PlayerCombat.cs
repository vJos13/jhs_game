using UnityEngine;
using Core.Interactions;

namespace Core.Systems.Combat
{
    public sealed class PlayerCombat : MonoBehaviour
    {
        [Header("Attack Settings")]
        [SerializeField] private KeyCode _attackKey = KeyCode.Mouse1;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _attackCooldown = 0.35f;

        [Header("Hitbox Detection")]
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private Vector2 _attackSize = new Vector2(0.65f, 0.55f);
        [SerializeField] private LayerMask _enemyLayers;

        private Animator _animator;
        private float _nextAttackTime;
        private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(_attackKey)) && Time.time >= _nextAttackTime)
            {
                PerformAttack();
                _nextAttackTime = Time.time + _attackCooldown;
            }
        }

        private void PerformAttack()
        {
            if (_animator != null)
            {
                _animator.SetTrigger(AttackTriggerHash);
            }

            if (_attackPoint == null) return;

            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(_attackPoint.position, _attackSize, 0f, _enemyLayers);

            foreach (var col in hitColliders)
            {
                if (col.gameObject == gameObject) continue;

                if (col.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.TakeDamage(_damage);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_attackPoint == null) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(_attackPoint.position, _attackSize);
        }
    }
}