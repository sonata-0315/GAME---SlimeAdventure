using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.UI
{
    public class LevelMenuController : MonoBehaviour
    {
        public void OnRestartClicked()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
