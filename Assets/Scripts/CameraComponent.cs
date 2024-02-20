using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Samurai
{
    public class CameraComponent : MonoBehaviour
    {
        [Inject]
        private Player _player;
        [SerializeField]
        private Vector3 _offset;
        [SerializeField]
        private float _divider = 300;
        [SerializeField]
        private float _limiter = 200;
        private Vector2 _res;


#region UnityMethods
        private void Start()
        {
            SetResolution();
        }
        private void Update()
        {
            var centerOffset = Mouse.current.position.ReadValue() - _res/2;
            var cameraOffset = centerOffset.sqrMagnitude > _limiter * _limiter ? centerOffset : Vector2.zero;
            cameraOffset = Vector3.ClampMagnitude(cameraOffset, cameraOffset.magnitude - _limiter) / _divider;

            transform.position = _player.transform.position + _offset + cameraOffset;

        }
#endregion


        private void SetResolution()
        {
            _res = new(Screen.currentResolution.width, Screen.currentResolution.height);
        }
    }
}