using MoreMountains.Tools;
using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class SoundManagerController : MonoBehaviour
    {
        private MMSoundManager _soundManager;
        private void Start()
        {
            _soundManager = GetComponent<MMSoundManager>();
            _soundManager.LoadSettings();
        }
    }
}