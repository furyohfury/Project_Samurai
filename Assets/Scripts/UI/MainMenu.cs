using UnityEditor;
using UnityEngine;
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

        #region UnityMethods
        #endregion

        public void ContinuePressed_UnityEvent()
        {
            // after saving system
        }
        public void NewGamePressed_UnityEvent()
        {
            // download startgame scene
        }
        public void SettingsPressed_UnityEvent()
        {
            _settingsMenu.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
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