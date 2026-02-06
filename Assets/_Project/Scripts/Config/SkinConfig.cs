using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Config
{
    [System.Serializable]
    public struct SkinData
    {
        public string SkinName;
        public Sprite UIIcon;
        public AnimatorOverrideController AnimatorOverride;
    }
    [CreateAssetMenu(menuName = "Config/Skin Config", fileName = "NewSkinConfig")]
    public class SkinConfig : ScriptableObject
    {
        [Header("Settings")]
        public int CoinsToUnlockOneSkin = 10;

        [Header("Skin Collection")]
        public List<SkinData> Skins;
    }
}
