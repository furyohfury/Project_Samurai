using System;
using System.Collections;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Samurai
{
    [RequireComponent(typeof(Player))]
    public class PlayerInput : UnitInput
    {
        [Inject]
        private Player _player;
        private PlayerControls _playerControls;
        [SerializeField]
        private MeshRenderer _attackKatana;
        [SerializeField]
        private MeshRenderer _sheathedKatana;
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
            _playerControls.PlayerMap.PickWeapon.performed += _player.EquipPickableWeapon;
            // _playerControls.PlayerMap.MeleeAttack.performed += _player.MeleeAttack;
            _playerControls.PlayerMap.MeleeAttack.performed += MeleeAttackAnimation;
        }
        protected override void Update()
        {            
            Vector2 movement = _playerControls.PlayerMap.Movement.ReadValue<Vector2>();
            MoveDirection = new Vector3(movement.x, 0, movement.y);
            // Moving animation happens after defining MD
            base.Update();
        }
        private void OnDisable()
        {
            _playerControls.PlayerMap.Shoot.performed -= UnitShootAnimation;
            _playerControls.Disable();
        }
        #endregion
        public void SetAnimatorController(AnimatorController controller) // mb redo in controller layers
        {
            UnitAnimator.runtimeAnimatorController = controller;
        }
        protected override void OnMeleeAttackAnimationStarted_UnityEvent()
        {
            base.OnMeleeAttackAnimationStarted_UnityEvent();
            Unit.UnitWeapon.CanShoot = false;
            _sheathedKatana.enabled = false;
            _attackKatana.enabled = true;
            _player.UnitWeapon.gameObject.SetActive(false);
        }
        protected override void OnMeleeAttackAnimationEnded_UnityEvent()
        {
            base.OnMeleeAttackAnimationEnded_UnityEvent();
            Unit.UnitWeapon.CanShoot = true;
            _attackKatana.enabled = false;
            _sheathedKatana.enabled = true;
            _player.UnitWeapon.gameObject.SetActive(true);
        }
        protected override void OnMeleeAttackSlashAnimationStarted_UnityEvent()
        {
            base.OnMeleeAttackSlashAnimationStarted_UnityEvent();
        }
        protected override void OnMeleeAttackSlashAnimationEnded_UnityEvent()
        {
            base.OnMeleeAttackSlashAnimationEnded_UnityEvent();

        }
    }
}