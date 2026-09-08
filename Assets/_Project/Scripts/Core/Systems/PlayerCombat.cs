using UnityEngine;
using Core.Interactions;
using Gameplay.Environment;

namespace Core.Systems.Combat
{
    public sealed class PlayerCombat : MonoBehaviour
    {
        [Header("Attack Settings")]
        [SerializeField] private KeyCode _attackKey = KeyCode.Mouse1;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _attackCooldown = 0.25f;

        [Header("Hitbox Detection")]
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private Vector2 _attackSize = new Vector2(0.55f, 0.45f);
        [SerializeField] private LayerMask _enemyLayers = ~0;

        private Animator _animator;
        private float _nextAttackTime;
        private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            bool attackInput = Input.GetMouseButtonDown(1) || 
                               Input.GetMouseButtonDown(0) || 
                               Input.GetKeyDown(_attackKey) || 
                               Input.GetKeyDown(KeyCode.J);

            if (attackInput && Time.time >= _nextAttackTime)
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

            Vector2 center = _attackPoint != null ? (Vector2)_attackPoint.position : (Vector2)transform.position;

            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(center, _attackSize, 0f, _enemyLayers);

            foreach (var col in hitColliders)
            {
                if (col.gameObject == gameObject) continue;

                if (col.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.TakeDamage(_damage);
                }
                else if (col.TryGetComponent<TrainingDummy>(out var dummy))
                {
                    dummy.TakeDamage(_damage);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Vector2 center = _attackPoint != null ? (Vector2)_attackPoint.position : (Vector2)transform.position;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(center, _attackSize);
        }
    }
}