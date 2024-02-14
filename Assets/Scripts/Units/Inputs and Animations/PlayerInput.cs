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
    public class PlayerInput : UnitInput, IAttackRange, IAttackMelee, IInputRange, IInputMelee
    {
        [Inject]
        private Player _player;
        private PlayerControls _playerControls;
        [SerializeField]
        private MeshRenderer _attackKatana;
        [SerializeField]
        private MeshRenderer _sheathedKatana;

        [SerializeField]
        private RangeWeapon _rangeWeapon;
        public RangeWeapon RangeWeapon { get => _rangeWeapon; private set => _rangeWeapon = value; }
        public bool CanShoot { get; private set; } = true;


        [SerializeField]
        private MeleeWeapon _meleeWeapon;
        public MeleeWeapon MeleeWeapon { get => _meleeWeapon; private set => _meleeWeapon = value; }
        public bool CanHit { get; private set; } = true;
        private float _meleeAttackCooldown = 8;
        public float MeleeAttackCooldown {get => _meleeAttackCooldown; private set => _meleeAttackCooldown = value;}
        

        [SerializeField]
        private Collider _meleeAttackHitbox; // todo mb udalit field
        public Collider MeleeAttackHitbox
        {
            get => _meleeAttackHitbox;
            set
            {
                _meleeAttackHitbox = value;
            }
        }

        private bool _inMeleeAttack = false;
        public bool InMeleeAttack
        {
            get => _inMeleeAttack;
            private set
            {
                if (value == true)
                {
                    _inMeleeAttack = true;
                    (Unit as IAttackRange).RangeWeapon.CanShoot = false;
                    CanMove = false;
                    CanHit = false;
                }
                else
                {
                    _inMeleeAttack = false;
                    (Unit as IAttackRange).RangeWeapon.CanShoot = true;
                    CanMove = true;
                    CanHit = true;
                }                
            }            
        }

        #region Unity_methods
        protected override void Awake()
        {
            base.Awake();
            _playerControls = new();
            if (RangeWeapon == null) RangeWeapon = GetComponentInChildren<RangeWeapon>();
            if (MeleeWeapon == null) MeleeWeapon = GetComponentInChildren<MeleeWeapon>();
            if (MeleeAttackHitbox == null) MeleeAttackHitbox = MeleeWeapon.GetComponentInChildren<Collider>();
        }
        protected override void Start()
        {
            base.Start();
        }
        private void OnEnable()
        {
            _playerControls.Enable();
            _playerControls.PlayerMap.Shoot.performed += Shoot;
            _playerControls.PlayerMap.BlueColor.performed += (cb) => _player.ChangeColor(PhaseColor.Blue);
            _playerControls.PlayerMap.RedColor.performed += (cb) => _player.ChangeColor(PhaseColor.Red);
            _playerControls.PlayerMap.PickWeapon.performed += _player.EquipPickableWeapon;
            // _playerControls.PlayerMap.MeleeAttack.performed += _player.MeleeAttack;
            _playerControls.PlayerMap.MeleeAttack.performed += MeleeAttack;
        }
        protected override void Update()
        {
            
            base.Update();
        }
        protected override void FixedUpdate()
        {
            Vector2 movement = _playerControls.PlayerMap.Movement.ReadValue<Vector2>();
            MoveDirection = new Vector3(movement.x, 0, movement.y);
            // Moving animation happens after defining MD
            base.FixedUpdate();
        }
        private void OnDisable()
        {
            _playerControls.PlayerMap.Shoot.performed -= Shoot;
            _playerControls.PlayerMap.PickWeapon.performed -= _player.EquipPickableWeapon;
            _playerControls.PlayerMap.MeleeAttack.performed -= MeleeAttack;
            _playerControls.Disable();
        }
        #endregion
        public void SetAnimatorController(AnimatorController controller) // mb redo in controller layers
        {
            UnitAnimator.runtimeAnimatorController = controller;
        }        
        public void OnShootAnimationStarted_UnityEvent() { }
        public void OnShootAnimationEnded_UnityEvent() { }

        public void Shoot(CallbackContext _) => Shoot();
        public void Shoot()
        {
            if (this.CanShoot && (Unit as IAttackRange).RangeWeapon.CanShoot)
            {
                UnitAnimator.SetTrigger("Shoot");
                (Unit as IAttackRange).Shoot();
            }
        }

        public void MeleeAttack(CallbackContext _) => MeleeAttack();
        public void MeleeAttack()
        {
            if (CanHit) 
            {
                UnitAnimator.SetTrigger("MeleeAttack");
                InMeleeAttack = true;
                StartCoroutine(MeleeAttackCD());
            }
        }
        private IEnumerator MeleeAttackCD()
        {
            CanHit = false;
            yield return new WaitForSeconds(MeleeAttackCooldown);
            CanHit = true;
        }

#region UnityEvents
        public void OnMeleeAttackAnimationStarted_UnityEvent()
        {
            InMeleeAttack = true;

            _sheathedKatana.enabled = false;
            _attackKatana.enabled = true;
            _player.RangeWeapon.gameObject.SetActive(false);
        }
        public void OnMeleeAttackAnimationEnded_UnityEvent()
        {
            InMeleeAttack = false;

            _attackKatana.enabled = false;
            _sheathedKatana.enabled = true;
            _player.RangeWeapon.gameObject.SetActive(true);
        }
        public void OnMeleeAttackSlashAnimationStarted_UnityEvent()
        {
            MeleeAttackHitbox.enabled = true;            
        }
        public void OnMeleeAttackSlashAnimationEnded_UnityEvent()
        {
            MeleeAttackHitbox.enabled = false;
            MeleeWeapon.Parrying = false;
        }
        #endregion
    }
}