using UnityEngine;
using Core.Interactions;

namespace Gameplay.Environment
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class SoulOrbCollectable : MonoBehaviour, ICollectable
    {
        [SerializeField] private int _soulValue = 1;

        private void Reset()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Collect();
            }
        }

        public void Collect()
        {
            Debug.Log($"[Gameplay] Orbe recolectado (+{_soulValue} almas).");
            Destroy(gameObject);
        }
    }
}
