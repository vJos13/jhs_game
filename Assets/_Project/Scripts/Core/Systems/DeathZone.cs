using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Systems
{
    public sealed class DeathZone : MonoBehaviour
    {
        [SerializeField] private string _playerTag = "Player";

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(_playerTag) || other.name.Contains("Player"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.35f);
            var col = GetComponent<BoxCollider2D>();
            if (col != null)
            {
                Gizmos.DrawCube(transform.position + (Vector3)col.offset, col.size);
            }
        }
    }
}