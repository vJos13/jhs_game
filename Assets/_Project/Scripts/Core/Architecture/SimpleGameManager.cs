using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Architecture
{
    public sealed class SimpleGameManager : MonoBehaviour
    {
        [SerializeField] private KeyCode _restartKey = KeyCode.R;

        private void Update()
        {
            if (Input.GetKeyDown(_restartKey))
            {
                RestartPrototype();
            }
        }

        public void RestartPrototype()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.buildIndex);
        }
    }
}