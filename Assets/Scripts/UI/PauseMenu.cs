using MoreMountains.Feedbacks;
using System;
using UnityEngine;
using Zenject;
namespace Samurai
{
    public class PauseMenu : MonoBehaviour
    {
        [Inject]
        private readonly PlayerUI _playerUI;
        [Inject]
        private readonly SettingsMenu _settingsMenu;
        [Inject]
        private readonly SaveLoadSceneAssistant _saveLoadSceneAssistant;

        [SerializeField]
        private MMF_Player _sceneSwitchToMainMenuFeedback;


        #region UnityMethods
        private void OnEnable()
        {
            _settingsMenu.OnSettingsBackMenuButtonPressed += SettingsClosed;
        }
        private void OnDisable()
        {
            _settingsMenu.OnSettingsBackMenuButtonPressed -= SettingsClosed;
        }
        #endregion

        public void ContinuePressed_UnityEvent()
        {
            _playerUI.Paused();
        }

        public void ReloadCheckpointPressed_UnityEvent()
        {
            _saveLoadSceneAssistant.LoadLastCheckpoint();
        }


        public void SettingsPressed_UnityEvent()
        {
            _settingsMenu.gameObject.SetActive(true);
            this.GetComponent<Canvas>().enabled = false;
        }
        private void SettingsClosed()
        {
            _settingsMenu.gameObject.SetActive(false);
            this.GetComponent<Canvas>().enabled = true;
        }


        public void MainMenuPressed_UnityEvent()
        {
            _sceneSwitchToMainMenuFeedback?.PlayFeedbacks();
        }
    }
}