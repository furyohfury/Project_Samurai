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
        private void Awake()
        {
            _playerControls = new();            
        }
        private void OnEnable()
        {
            _playerControls.Enable();
        }
        private void Update()
        {
            // Moving
            Vector2 movement = _playerControls.PlayerMap.Movement.ReadValue<Vector2>();
            MoveDirection = movement;            
        }
        private void OnDisable()
        {
            _playerControls.Disable();
        }
    }
}