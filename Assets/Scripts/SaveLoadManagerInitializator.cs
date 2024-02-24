using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class SaveLoadManagerInitializator : MonoBehaviour
    {
        private void OnEnable()
        {
            SaveLoadManager.SaveLoadManagerInitialization(true);
        }
        void Start()
        {
            
        }
        private void OnDisable()
        {
            SaveLoadManager.SaveLoadManagerInitialization(false);
        }
    }
}