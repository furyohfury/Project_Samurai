using System.Collections;
using UnityEditor;
using UnityEditor.Animations;
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
        protected override void Start()
        {
            base.Start();            
        }
        private void OnEnable()
        {
            _playerControls.Enable();
            _playerControls.PlayerMap.Shoot.performed += UnitShootAnimation;
            _playerControls.PlayerMap.BlueColor.performed += (cb) => _player.ChangeColor(PhaseColor.Blue);
            _playerControls.PlayerMap.RedColor.performed += (cb) => _player.ChangeColor(PhaseColor.Red);
            _playerControls.PlayerMap.PickWeapon.performed += _player.EquipWeapon;
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
        public void SetAnimatorController(AnimatorController controller)
        {
            UnitAnimator.runtimeAnimatorController = controller;
        }
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