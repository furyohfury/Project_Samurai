using MoreMountains.Feedbacks;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
namespace Samurai
{
    public class MainMenu : MonoBehaviour
    {
        [Inject]
        private SaveLoadManager SaveLoadManager;

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
            // _saveLoadSceneAssistant.UseSaveFile = false;
            _settingsMenu.OnSettingsBackMenuButtonPressed += SettingsPressed_UnityEvent;
        }
        private void Start()
        {
            _bgMusicFeedback?.PlayFeedbacks();
            Debug.Log(Application.persistentDataPath);
            Application.targetFrameRate = 144;

            // SaveLoadManager.SaveLoadManagerInitialization(true);
        }
        private void OnDisable()
        {
            _settingsMenu.OnSettingsBackMenuButtonPressed -= SettingsPressed_UnityEvent;
        }
        #endregion

        public void ContinuePressed_UnityEvent()
        {
            SaveLoadManager.ChangeScene(LoadingType.ContinueFromMainMenu);
        }
        private IEnumerator ContinueLoading(string sceneName)
        {
            SceneManager.LoadScene("AdditiveLoadingScreen", LoadSceneMode.Additive);
            AsyncOperation startGameLoading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!startGameLoading.isDone)
            {
                yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("AdditiveLoadingScreen"),
                UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("MainMenuScene"),
                UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }

        public void NewGamePressed_UnityEvent()
        {
            if (!_sureNewGameMenu.activeInHierarchy) _sureNewGameMenu.SetActive(true);
        }

        public void SureMenuYesPressed_UnityEvent()
        {
            SaveLoadManager.ChangeScene(LoadingType.NewGameFromMainMenu);
        }
        private IEnumerator StartGameSceneLoading()
        {
            var loadingScreenLoad = SceneManager.LoadSceneAsync("AdditiveLoadingScreen", LoadSceneMode.Additive);
            while (!loadingScreenLoad.isDone)
            {
                yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("AdditiveLoadingScreen"));
            AsyncOperation startGameLoading = SceneManager.LoadSceneAsync("StartGameScene", LoadSceneMode.Additive);
            while (!startGameLoading.isDone)
            {
                yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("StartGameScene"));
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("AdditiveLoadingScreen"),
                UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("MainMenuScene"),
                UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }

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