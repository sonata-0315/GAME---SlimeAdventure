using UnityEngine;

namespace Platformer.Mechanics
{
    public class LevelEndDoor : MonoBehaviour
    {
        [Header("UI Reference")]
        [Tooltip("for stage clear panel")]
        public GameObject stageClearPanel;

        private bool _isTriggered = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isTriggered) return;

            //check if the door touch the Player
            if (collision.CompareTag("Player"))
            {
                _isTriggered = true;
                ShowClearPanel();
            }
        }

        private void ShowClearPanel()
        {
            if (stageClearPanel != null)
            {
                //activate and show the panel
                stageClearPanel.SetActive(true);

                //stop the game time
                Time.timeScale = 0f;
            }
            else
            {
                Debug.LogError("No Stage Clear Panel");
            }
        }
    }
}
