using MoreMountains.Feedbacks;
using UnityEngine;
using Zenject;
namespace Samurai
{
    public class LoseMenu : MonoBehaviour
    {
        [Inject]
        private readonly SaveLoadSceneAssistant _saveLoadSceneAssistant;


        [SerializeField]
        private MMF_Player _sceneSwitchToMainMenuFeedback;

        public void ReloadCheckpointPressed_UnityEvent()
        {
            Time.timeScale = 1;
            _saveLoadSceneAssistant.LoadLastCheckpoint();
        }

        public void MainMenuPressed_UnityEvent()
        {
            Time.timeScale = 1;
            // _sceneSwitchToMainMenuFeedback?.PlayFeedbacks();
            _saveLoadSceneAssistant.LoadMainMenu();
        }
    }
}