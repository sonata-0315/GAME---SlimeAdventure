using UnityEngine;
using Platformer.Mechanics;
using Platformer.Core;

namespace Platformer.GamePlay
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private GameObject _collectEffect;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var skinManager = ServiceLocator.Get<SkinManager>();

                if (skinManager != null)
                {
                    skinManager.CollectCoin();
                }

                if (_collectEffect != null)
                    Instantiate(_collectEffect, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }
}