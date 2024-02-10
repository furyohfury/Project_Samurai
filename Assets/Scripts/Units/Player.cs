using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.Windows;
using Zenject;
using static UnityEngine.InputSystem.InputAction;
namespace Samurai
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player : Unit
    {
        public Vector3 TestShit;


        private Camera _camera;
        private float _cameraOffset;


        /* [SerializeField]
        private PlayerWeapon _weapon;
        public PlayerWeapon Weapon
        {
            get => _weapon;
            set => _weapon = value;
        } */


        [Inject]
        private DefaultPlayerGunPool _defaultGunPool;
        private DefaultPlayerWeapon _defaultPlayerWeapon;

        [SerializeField]
        private Transform _weaponSlot;

        private Weapon _pickableWeapon;
        public Weapon PickableWeapon
        {
            get => _pickableWeapon;
            private set => _pickableWeapon = value;
        }

        #region Unity_Methods
        protected override void Awake()
        {
            base.Awake();
            UnitInput = GetComponent<PlayerInput>();
        }
        protected override void Start()
        {
            base.Start();
            if (_camera == null) _camera = Camera.main;
            _defaultPlayerWeapon = GetComponentInChildren<DefaultPlayerWeapon>();
        }
        protected override void Update()
        {
            base.Update();

            // Facing cursor
            _cameraOffset = Vector3.Distance(transform.position, _camera.transform.position); //todo fix. Must be constant Y
            Vector3 cursorPosition = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, _cameraOffset);
            cursorPosition = _camera.ScreenToWorldPoint(cursorPosition);
            cursorPosition = new Vector3(cursorPosition.x, this.transform.position.y, cursorPosition.z);
            TestShit = cursorPosition;
            transform.LookAt(cursorPosition);
        }
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.TryGetComponent(out Weapon weapon) && weapon.IsPickable && weapon.Owner == null)
            {
                PickableWeapon = weapon;
            }
            else if (other.TryGetComponent(out Projectile proj))
            {
                Destroy(proj);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            // if (other.TryGetComponent(out Weapon weapon) && weapon.IsPickable && weapon.Owner == null)
            if (PickableWeapon != null && other.transform == PickableWeapon.transform && UnitWeapon != PickableWeapon)
            {
                PickableWeapon = null;
            }
        }
        #endregion
        public override void ChangeColor(PhaseColor color)
        {
            base.ChangeColor(color);
            // Notification to obstacles to change color too
            OnPlayerSwapColor?.Invoke(color);
        }
        public override void Die()
        {
            UnitInput.enabled = false;
            Time.timeScale = 0;
            Instantiate(Resources.Load<GameObject>("UI/GameOverScreen"));
        }
        #region Guns
        public override void UnitShoot()
        {
            base.UnitShoot();
        }
        
        public void EquipPickableWeapon(CallbackContext _)
        {
            if (PickableWeapon == null) return;

            UnitWeapon = PickableWeapon;
            // Throw away empty gun
            UnitWeapon.OnBulletsEnded += UnequipPickableWeapon;

            _defaultPlayerWeapon.gameObject.SetActive(false);
            UnitWeapon.transform.parent = _weaponSlot;

            UnitWeapon.transform.SetLocalPositionAndRotation(UnitWeapon.WeaponPositionWhenPicked, Quaternion.Euler(UnitWeapon.WeaponRotationWhenPicked));

            ((PlayerInput)UnitInput).SetAnimatorController(UnitWeapon.AnimController);
            UnitWeapon.Equipped(this);
            PickableWeapon = null;
        }
        public void UnequipPickableWeapon()
        {
            Destroy(UnitWeapon.gameObject);
            _defaultPlayerWeapon.gameObject.SetActive(true);
            UnitWeapon = _defaultPlayerWeapon;
            ((PlayerInput)UnitInput).SetAnimatorController(_defaultPlayerWeapon.AnimController);
            _defaultPlayerWeapon.Equipped(this);            
        }
        #endregion
        #region Melee
        public override void MeleeAttack(CallbackContext _)
        {
            base.MeleeAttack(_);
        }
        #endregion
        public event ChangeColorHandle OnPlayerSwapColor;
    }
}