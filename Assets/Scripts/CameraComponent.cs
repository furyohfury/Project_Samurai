using System.Collections;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class CameraComponent : MonoBehaviour
    {
        [Inject]
        private Player _player;
        [SerializeField]
        private Vector3 _offset;

        private void Update()
        {
            transform.position = _player.transform.position + _offset;
        }
    }
}