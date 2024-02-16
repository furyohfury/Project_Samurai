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
    [RequireComponent(typeof(PlayerInput))]
    public class Player : Unit, IAttackRange, IAttackMelee
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
            MeleeWeapon.OnParry += Parrying;
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
            FaceCursor();
        }
        protected void OnDisable()
        {
            MeleeWeapon.OnParry -= Parrying;
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
            if ((weapon.Owner as Enemy != null) && (!this.MeleeWeapon.Parrying))
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
        protected override void Movement()
        {
            // Walking. Player moves ignoring timescale
            if (UnitInput.MoveDirection != Vector3.zero && UnitInput.CanMove)
            {
                if (CharController.isGrounded) CharController.Move(UnitStats.MoveSpeed * Time.fixedDeltaTime * (1 / Time.timeScale) * new Vector3(UnitInput.MoveDirection.x, 0, UnitInput.MoveDirection.z));
                else CharController.Move(Time.fixedDeltaTime * (1 / Time.timeScale) * (UnitStats.MoveSpeed * new Vector3(UnitInput.MoveDirection.x, 0, UnitInput.MoveDirection.z) + 9.8f * Vector3.down));
            }
        }
        public override void Die()
        {
            UnitInput.enabled = false;
            Time.timeScale = 0;
            Instantiate(Resources.Load<GameObject>("UI/GameOverScreen"));
        }
        #region Guns
        public void Shoot()
        {
            RangeWeapon.Shoot();
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

            ((PlayerInput)UnitInput).SetAnimatorController(RangeWeapon.AnimController);
            RangeWeapon.Equipped(this);
            PickableWeapon = null;
        }
        public void UnequipPickableWeapon()
        {
            Destroy(RangeWeapon.gameObject);
            _defaultPlayerWeapon.gameObject.SetActive(true);
            RangeWeapon = _defaultPlayerWeapon;
            ((PlayerInput)UnitInput).SetAnimatorController(_defaultPlayerWeapon.AnimController);
            _defaultPlayerWeapon.Equipped(this);
        }
        #endregion
        #region Melee       
        private Coroutine _parryCor;
        [SerializeField, Tooltip("Time for slow-mo after parry")]
        private float _parrySlowmoTime;
        [SerializeField, Tooltip("Coefficient for timescale")]
        private float _slowMoMultiplier;
        private Parrying()
        {
            if (_parryCor == null) _parryCor = StartCoroutine(ParryingCoroutine());
        }
        private IEnumerator ParryingCoroutine()
        {
            Time.timeScale = _slowMoMultiplier; // todo make player not slowed
            yield return new WaitForSeconds(_parrySlowmoTime * _slowMoMultiplier);
            Time.timeScale = 1;
            _parryCor = null
        }
        
        #endregion
        public event ChangeColorHandle OnPlayerSwapColor;
    }
}