using UnityEngine.SceneManagement;
using UnityEngine;

namespace AfterDusk
{
    public class GameManager : MonoBehaviour
    {
        private void OnEnable()
        {
            InputManager.OnRestartScenePressed += RestartScene;
        }

        private void OnDisable()
        {
            InputManager.OnRestartScenePressed -= RestartScene;
        }

        private void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
