using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class SaveLoadManagerInitializator : MonoBehaviour
    {
        private void OnEnable()
        {
            // Creating save file if empty and subscribing on scene change
            // SaveLoadManager.SaveLoadManagerInitialization(true);
        }
        private void OnDisable()
        {
            // SaveLoadManager.SaveLoadManagerInitialization(false);
        }
    }
}