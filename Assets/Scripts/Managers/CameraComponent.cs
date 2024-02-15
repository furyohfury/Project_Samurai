using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Samurai {
    public class CameraComponent : MonoBehaviour
    {
        [Inject]
        private Player Player;
        private void FixedUpdate()
        {

        }
    }
}