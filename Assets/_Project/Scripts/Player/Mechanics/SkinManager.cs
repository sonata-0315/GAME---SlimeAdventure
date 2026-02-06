using System.Collections.Generic;
using UnityEngine;
using Platformer.Config;
using Platformer.Core;

namespace Platformer.Mechanics
{
    public class SkinManager : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private SkinConfig _config;

        [Header("References")]
        [SerializeField] private Animator _playerAnimator;

        public int CurrentCoins { get; private set; } = 0;
        public int UnlockedSkinCount { get; private set; } = 1;

        private void Awake()
        {
            ServiceLocator.Register<SkinManager>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<SkinManager>();
        }

        public void Start()
        {
            EquipSkin(0);
        }

        public void CollectCoin()
        {
            CurrentCoins++;
            CheckUnlock();
        }

        private void CheckUnlock()
        {
            int skinShouldBeUnlocked = 1 + (CurrentCoins / _config.CoinsToUnlockOneSkin);
            skinShouldBeUnlocked = Mathf.Clamp(skinShouldBeUnlocked, 1, _config.Skins.Count);

            if (skinShouldBeUnlocked > UnlockedSkinCount)
            {
                UnlockedSkinCount = skinShouldBeUnlocked;
                Debug.Log($"Unlock new Slime! You are {UnlockedSkinCount}");
            }
        }

        public void EquipSkin(int index)
        {
            if (index >= 0 && index < _config.Skins.Count)
            {
                if (index < UnlockedSkinCount)
                {
                    var skinData = _config.Skins[index];

                    if (skinData.AnimatorOverride != null)
                    {
                        _playerAnimator.runtimeAnimatorController = skinData.AnimatorOverride;
                    }

                    Debug.Log($"Change to {skinData.SkinName}");
                }
            }
        }

        public int GetTotalSkinCount() => _config.Skins.Count;
        public Sprite GetSkinSprite(int index) => _config.Skins[index].UIIcon;

    }
}