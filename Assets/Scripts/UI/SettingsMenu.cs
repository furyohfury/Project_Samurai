using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Samurai
{
    public class SettingsMenu : MonoBehaviour
    {
        [Inject]
        private readonly MMSoundManager _soundManager;

        [SerializeField]
        private Slider _masterVolumeSlider;
        [SerializeField]
        private Slider _musicVolumeSlider;
        [SerializeField]
        private Slider _sfxVolumeSlider;
        [SerializeField]
        private Slider _UIVolumeSlider;

        [SerializeField]
        private TMP_Dropdown _languageDropdown;
        [SerializeField]
        private Button _backButton;

        #region UnityMethods
        private void Awake()
        {
            Initialize();
        }
        private void Start()
        {
            var smSettings = _soundManager.settingsSo.Settings;
            _masterVolumeSlider.value = smSettings.MasterVolume;
            _musicVolumeSlider.value = smSettings.MusicVolume;
            _sfxVolumeSlider.value = smSettings.SfxVolume;
            _UIVolumeSlider.value = smSettings.UIVolume;
        }
        #endregion
        
        private void Initialize()
        {
            if (_masterVolumeSlider == null) _masterVolumeSlider = GetComponentInChildren<Slider>();
            if (_musicVolumeSlider == null) _musicVolumeSlider = GetComponentInChildren<Slider>();
            if (_sfxVolumeSlider == null) _sfxVolumeSlider = GetComponentInChildren<Slider>();
            if (_UIVolumeSlider == null) _UIVolumeSlider = GetComponentInChildren<Slider>();
            if (_languageDropdown == null) _languageDropdown = GetComponentInChildren<TMP_Dropdown>();
            if (_backButton == null) _backButton = GetComponentInChildren<Button>();
        }

        public void MasterVolumeChanged_UnityEvent(float v)
        {
            _soundManager.SetVolumeMaster(v);
        }
        public void MusicVolumeChanged_UnityEvent(float v)
        {
            _soundManager.SetVolumeMusic(v);
        }
        public void SFXVolumeChanged_UnityEvent(float v)
        {
            _soundManager.SetVolumeSfx(v);
        }
        public void UIVolumeChanged_UnityEvent(float v)
        {
            _soundManager.SetVolumeUI(v);
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