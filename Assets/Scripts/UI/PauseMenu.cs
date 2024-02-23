using UnityEngine;
using Zenject;
namespace Samurai
{
    public class PauseMenu : MonoBehaviour
    {
        [Inject]
        private PlayerUI PlayerUI;
        [Inject]
        private SettingsMenu SettingsMenu;

        public void ContinuePressed_UnityEvent()
        {
            PlayerUI.Paused();
        }
        public void SettingsPressed_UnityEvent()
        {
            SettingsMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        public void MainMenuPressed_UnityEvent()
        {
            // todo scene switch
        }
    }
}