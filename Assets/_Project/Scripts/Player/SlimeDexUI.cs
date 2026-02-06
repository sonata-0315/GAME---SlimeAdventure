using UnityEngine;
using UnityEngine.UI;
using Platformer.Mechanics;
using Platformer.Core;
using Platformer.Player;

namespace Platformer.UI
{
    public class SlimeDexUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject _panel;
        [SerializeField] private Transform _gridContainer;
        [SerializeField] private GameObject _slotPrefab;

        [Header("Visuals")]
        [SerializeField] private Color _lockedColor = Color.black;
        [SerializeField] private Color _unlockedColor = Color.white;

        private bool _isOpen = false;
        private InputReader _input;

        private void Start()
        {
            _panel.SetActive(false);

            _input = ServiceLocator.Get<InputReader>();

            if (_input != null)
            {
                _input.OpenDexEvent += TogglePanel;
            }
        }

        private void OnDestroy()
        {
            if (_input != null)
            {
                _input.OpenDexEvent -= TogglePanel;
            }
        }

        private void TogglePanel()
        {
            _isOpen =  !_isOpen;
            _panel.SetActive(_isOpen);

            if (_isOpen)
            {
                RefreshUI();
            }
        }

        private void RefreshUI()
        {
            foreach (Transform child in _gridContainer)
            {
                Destroy(child.gameObject);
            }

            var skinManager = ServiceLocator.Get<SkinManager>();

            if (skinManager== null) return;

            int totalSkins = skinManager.GetTotalSkinCount();
            int unlockedCount = skinManager.UnlockedSkinCount;

            for (int i = 0; i < totalSkins; i++)
            {
                GameObject newSlot = Instantiate(_slotPrefab, _gridContainer);
                Image icon = newSlot.GetComponent<Image>();

                icon.sprite = skinManager.GetSkinSprite(i);

                bool isUnlocked = i < unlockedCount;
                icon.color = isUnlocked ? _unlockedColor : _lockedColor;

                if (isUnlocked)
                {
                    int skinIndex = i;
                    Button btn = newSlot.GetComponent<Button>();
                    if (btn != null)
                    {
                        btn.onClick.AddListener(() =>
                        {
                            skinManager.EquipSkin(skinIndex);
                        });
                    }
                }
            }
        }
    }
}
