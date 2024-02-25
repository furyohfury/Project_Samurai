using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Samurai
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField]
        private Slider _volumeSlider;
        [SerializeField]
        private TMP_Dropdown _languageDropdown;
        [SerializeField]
        private Button _backButton;

        private void Awake()
        {
            if (_volumeSlider == null) _volumeSlider = GetComponentInChildren<Slider>();
            if (_languageDropdown == null) _languageDropdown = GetComponentInChildren<TMP_Dropdown>();
        }

        public void VolumeChanged_UnityEvent(float v)
        {
            //todo if have time
        }
        
        public void LanguageChanged_UnityEvent(int langInd)
        {
            //todo if have time
        }

        public void BackButtonPressed_UnityEvent()
        {
            OnSettingsBackMenuButtonPressed?.Invoke();
        }

        public event SimpleHandle OnSettingsBackMenuButtonPressed;
    }
}