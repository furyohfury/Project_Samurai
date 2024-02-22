using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.Windows;
using Zenject;
using static UnityEngine.InputSystem.InputAction;
namespace Samurai
{
    /* [RequireComponent(typeof(PlayerInput))]
    public class PlayerOld : Unit, IAttackRange, IAttackMelee
    {
        public Vector3 TestShit;

        [SerializeField]
        private RangeWeapon _rangeWeapon;
        public RangeWeapon RangeWeapon { get => _rangeWeapon; private set => _rangeWeapon = value; }

        private Camera _camera; // todo w/ zenject
        private float _cameraOffset;

        [Inject]
        private DefaultPlayerGunPool _defaultGunPool;
        private DefaultPlayerWeapon _defaultPlayerWeapon;



        private RangeWeapon _pickableWeapon;
        public RangeWeapon PickableWeapon
        {
            get => _pickableWeapon;
            private set => _pickableWeapon = value;
        }

        [SerializeField]
        private MeleeWeapon _meleeWeapon;
        public MeleeWeapon MeleeWeapon { get => _meleeWeapon; private set => _meleeWeapon = value; }      


        #region Unity_Methods
        protected override void Awake()
        {
            base.Awake();
            UnitInput = GetComponent<PlayerInput>();
            RangeWeapon = GetComponentInChildren<RangeWeapon>();
            MeleeWeapon = GetComponentInChildren<MeleeWeapon>();
        }
        protected void OnEnable()
        {
            // MeleeWeapon.OnParry += Parrying;
        }
        protected override void Start()
        {
            base.Start();
            if (_camera == null) _camera = Camera.main;
            _defaultPlayerWeapon = GetComponentInChildren<DefaultPlayerWeapon>(true);
        }
        protected override void Update()
        {
            base.Update();
            FaceCursor();
        }
        protected void OnDisable()
        {
            // MeleeWeapon.OnParry -= Parrying;
        }
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.TryGetComponent(out RangeWeapon weapon) && weapon.IsPickable && weapon.Owner == null)
            {
                PickableWeapon = weapon;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            // if (other.TryGetComponent(out Weapon weapon) && weapon.IsPickable && weapon.Owner == null)
            if (PickableWeapon != null && other.transform == PickableWeapon.transform && RangeWeapon != PickableWeapon) //todo maybe delete last check
            {
                PickableWeapon = null;
            }
        }
        #endregion


        protected override void GetDamagedByMelee(MeleeWeapon weapon)
        {
            if ((weapon.Owner as Enemy != null) && !this.MeleeWeapon.Parrying && (UnitInput as PlayerInput)._parryCor == null)
            {
                GetDamaged(weapon.Damage);
            }
        }
        public override void ChangeColor(PhaseColor color)
        {
            base.ChangeColor(color);
            // Notification to obstacles to change color too
            OnPlayerSwapColor?.Invoke(color);
        }
        private void FaceCursor()
        {
            // Facing cursor
            _cameraOffset = Vector3.Distance(transform.position, _camera.transform.position); //todo fix. Must be constant Y
            Vector3 cursorPosition = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, _cameraOffset);
            cursorPosition = _camera.ScreenToWorldPoint(cursorPosition);
            cursorPosition = new Vector3(cursorPosition.x, this.transform.position.y, cursorPosition.z);
            TestShit = cursorPosition;
            transform.LookAt(cursorPosition);
        }
        
        public override void Die()
        {
            UnitInput.enabled = false;
            Time.timeScale = 0;
            Instantiate(Resources.Load<GameObject>("UI/GameOverScreen"));
        }
        #region Range
        public void Shoot()
        {
            RangeWeapon.RangeAttack();
            if (RangeWeapon.GetType() != typeof(DefaultPlayerWeapon)) OnPlayerShot?.Invoke();
        }

        public void EquipPickableWeapon(CallbackContext _)
        {
            if (PickableWeapon == null) return;
            RangeWeapon = PickableWeapon;

            // Throw away empty gun
            RangeWeapon.OnBulletsEnded += UnequipPickableWeapon;

            _defaultPlayerWeapon.gameObject.SetActive(false);
            RangeWeapon.transform.parent = WeaponSlot;

            RangeWeapon.transform.SetLocalPositionAndRotation(RangeWeapon.WeaponPositionWhenPicked, Quaternion.Euler(RangeWeapon.WeaponRotationWhenPicked));

            ((PlayerInput)UnitInput).SetAnimatorController((UnityEditor.Animations.AnimatorController)RangeWeapon.AnimController);
            RangeWeapon.Equipped(this);
            PickableWeapon = null;

            if (Enum.TryParse(RangeWeapon.GetType().Name, true, out RangeWeaponEnum weapon))
            {
                OnPlayerChangedWeapon?.Invoke(weapon);
            }
            else Debug.LogWarning($"Player equipped weapon not in enum {typeof(RangeWeaponEnum)}");
        }
        public void UnequipPickableWeapon()
        {
            Destroy((UnityEngine.Object)RangeWeapon.gameObject);
            _defaultPlayerWeapon.gameObject.SetActive(true);
            RangeWeapon = _defaultPlayerWeapon;
            ((PlayerInput)UnitInput).SetAnimatorController(_defaultPlayerWeapon.AnimController);
            _defaultPlayerWeapon.Equipped(this);

            OnPlayerChangedWeapon?.Invoke(RangeWeaponEnum.DefaultPlayerWeapon);
        }
        #endregion

        

        public void MeleeAttack()
        {
            throw new NotImplementedException();
        }

        #endregion

        public event ChangeColorHandle OnPlayerSwapColor;
        public event RangeWeaponChangeHandle OnPlayerChangedWeapon;
        public event SimpleHandle OnPlayerShot;
    } */
}