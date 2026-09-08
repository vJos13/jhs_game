using UnityEngine;
using Core.Interactions;

namespace Gameplay.Environment
{
    public sealed class TrainingDummy : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _health = 3;

        public void TakeDamage(int damageAmount)
        {
            _health -= damageAmount;
            Debug.Log($"[Combate] ¡Enemigo golpeado! Daño recibido: {damageAmount}. Vida restante: {_health}");

            if (_health <= 0)
            {
                Debug.Log("[Combate] ¡Dummy destruido exitosamente!");
                Destroy(gameObject);
            }
        }
    }
}