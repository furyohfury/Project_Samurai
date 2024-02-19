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

        private void Start()
        {
            SetResolution();
        }
        private void Update()
        {
            var mouse = Mouse.current.position.ReadValue();
            // todo think on math.
            var xOffset = mouse.x - _res.x / 2 > _limiter ? mouse.x - _res.x / 2 - _limiter : 0;
            var yOffset = mouse.y - _res.y / 2 > _limiter ? mouse.y - _res.y / 2 - _limiter : 0;

            transform.position = _player.transform.position + _offset + new Vector3(xOffset, 0, yOffset) / _divider;

        }

        private void SetResolution()
        {
            _res = new(Screen.currentResolution.width, Screen.currentResolution.height);
        }
    }
}