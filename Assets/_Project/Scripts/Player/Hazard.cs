using UnityEngine;
using Platformer.Mechanics;

namespace Platformer.GamePlay
{
    public class Hazard : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            //check if the object is player or not
            if (other.CompareTag("Player"))
            {
                var playerRespawn = other.GetComponent<PlayerRespawn>();

                if (playerRespawn != null)
                {
                    playerRespawn.Die();
                }
            }
        }
    }
}
