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
        [Inject]
        private Player _player;
        private PlayerControls _playerControls;
        #region Unity_methods
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
            _playerControls.PlayerMap.Shoot.performed += UnitShootAnimation;
            _playerControls.PlayerMap.BlueColor.performed += (cb) => _player.ChangeColor(PhaseColor.Blue);
            _playerControls.PlayerMap.RedColor.performed += (cb) => _player.ChangeColor(PhaseColor.Red);

        }
        protected override void Update()
        {
            // Moving
            Vector2 movement = _playerControls.PlayerMap.Movement.ReadValue<Vector2>();
            MoveDirection = new Vector3(movement.x, 0, movement.y);
            base.Update();
        }
        private void OnDisable()
        {
            _playerControls.PlayerMap.Shoot.performed -= UnitShootAnimation;
            _playerControls.Disable();
        }
        #endregion
        private void OnDefGunShootAnimationStarted_UnityEvent()
        {
            CanShoot = false;
        }
        private void OnDefGunShootAnimationEnded_UnityEvent()
        {
            CanShoot = true;
        }
    }
}