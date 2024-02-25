using MoreMountains.Feedbacks;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Samurai
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private Button _continue;
        [SerializeField]
        private Button _newGame;
        [SerializeField]
        private Button _settings;
        [SerializeField]
        private Button _exit;
        [SerializeField, Space]
        private SettingsMenu _settingsMenu;

        [SerializeField]
        private GameObject _sureNewGameMenu;
        [SerializeField, Space]
        private MMF_Player _startGameSceneFeedback;
        [SerializeField]
        private MMF_Player _bgMusicFeedback;

        #region UnityMethods
        private void OnEnable()
        {
            _settingsMenu.OnSettingsBackMenuButtonPressed += SettingsPressed_UnityEvent;
        }
        private void Start()
        {
            _bgMusicFeedback?.PlayFeedbacks();
            Application.targetFrameRate = 60;
        }
        private void OnDisable()
        {
            _settingsMenu.OnSettingsBackMenuButtonPressed -= SettingsPressed_UnityEvent;
        }
        #endregion

        public void ContinuePressed_UnityEvent()
        {
            if (SaveLoadManager._saveData.count > 0) SaveLoadManager.LoadLastSave();
        }


        public void NewGamePressed_UnityEvent()
        {
            if (!_sureNewGameMenu.activeInHierarchy) _sureNewGameMenu.SetActive(true);
        }

        public void SureMenuYesPressed_UnityEvent()
        {
            // StartCoroutine(LoadGameStart());
            _startGameSceneFeedback?.PlayFeedbacks();
        }
        /* private IEnumerator LoadGameStart()
        {
            var loadedScene = SceneManager.LoadSceneAsync("StartGameScene", LoadSceneMode.Additive);
            SceneManager.LoadScene("LoadingScreen");
            while (!loadedScene.isDone)
            {
                yield return null;
            }
        }*/

        public void SureMenuNoPressed_UnityEvent()
        {
            _sureNewGameMenu.SetActive(false);
        }


        public void SettingsPressed_UnityEvent()
        {
            if (!_settingsMenu.gameObject.activeInHierarchy)
            {
                _settingsMenu.gameObject.SetActive(true);
            }
            else
            {
                _settingsMenu.gameObject.SetActive(false);
            }
        }


        public void ExitPressed_UnityEvent()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}