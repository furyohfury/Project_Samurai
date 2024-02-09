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

        [SerializeField]
        private Transform _weaponHand;

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
            if (other.TryGetComponent(out Weapon weapon) && weapon.IsPickable)
            {
                PickableWeapon = weapon;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Weapon weapon) && weapon.IsPickable)
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

        public override void UnitShoot()
        {
            base.UnitShoot();
        }
        public override void Die()
        {
            UnitInput.enabled = false;
            Time.timeScale = 0;
            Instantiate(Resources.Load<GameObject>("UI/GameOverScreen"));
        }
        public void EquipWeapon(CallbackContext _)
        {
            UnitWeapon.gameObject.SetActive(false);
            ((PlayerInput)UnitInput).SetAnimatorController(PickableWeapon.AnimController);
            PickableWeapon.transform.position = _weaponHand.position;
            // PickableWeapon.transform.rotation = Quaternion.Euler(new Vector3(55, this.transform.eulerAngles.y - 90, 0));
            // PickableWeapon.transform.eulerAngles = new Vector3(290, -35, 90);
            PickableWeapon.transform.parent = _weaponHand.transform;
            
            UnitWeapon = PickableWeapon;
            PickableWeapon.Equipped(this);
            PickableWeapon = null;

        }
        public event ChangeColorHandle OnPlayerSwapColor;
    }
}