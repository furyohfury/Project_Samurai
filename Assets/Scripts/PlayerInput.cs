using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Samurai
{
    [RequireComponent(typeof(Player))]
    public class PlayerInput : UnitInput
    {
        [Inject, SerializeField] //todo ostavit tolko zenject
        private Player _player;
        private PlayerControls _playerControls;
        protected override void Awake()
        {
            base.Awake();
            _playerControls = new();
        }
        private void Start()
        {

        }
        private void OnEnable()
        {
            _playerControls.Enable();
            _playerControls.PlayerMap.Shoot.performed += UnitShoot;
        }
        protected override void Update()
        {
            // Moving
            Vector2 movement = _playerControls.PlayerMap.Movement.ReadValue<Vector2>();
            MoveDirection = movement;
            base.Update();
        }
        private void OnDisable()
        {
            _playerControls.PlayerMap.Shoot.performed -= UnitShoot;
            _playerControls.Disable();
        }
    }
}