using UnityEngine;
namespace Samurai
{    
    public class Player : Unit, IRangeAttack
    {      
        private DefaultPlayerWeapon _defaultPlayerWeapon;

        [SerializeField]
        private RangeWeapon _rangeWeapon;
        public RangeWeapon RangeWeapon { get => _rangeWeapon; private set => _rangeWeapon = value; }       
        public bool CanShoot { get; set; } = true;

        #region UnityMethods
        protected void Start()
        {
            EquipRangeWeapon(_defaultPlayerWeapon);
        }
        #endregion

        #region GetDamaged
        public virtual void GetDamagedByMelee(MeleeWeapon weapon)
        {
            if (!Parried && (this.GetType() != weapon.Owner.GetType()) && (this as Enemy == null || weapon.Owner as Enemy == null))
            {
                UnitVisuals.GetDamagedByMelee();
                ChangeHP(-weapon.Damage);
            }
        }
        #endregion

        #region RangeAttack
        public void RangeAttack()
        {
            if (CanShoot && RangeWeapon.CanShoot) 
            {
                RangeWeapon.RangeAttack();
                (UnitVisuals as IRangeAttack).RangeAttack();
            }
            
        }
        #endregion
        
        
        #region PickableWeapon
        private RangeWeapon _pickableWeapon;
        public RangeWeapon PickableWeapon
        {
            get => _pickableWeapon; private set => _pickableWeapon = value;
        }
        public void SetPlayerPickableWeapon(RangeWeapon rweapon) => PickableWeapon = rweapon;

        public void EquipRangeWeapon(RangeWeapon rWeapon)
        {
            RangeWeapon = rWeapon;
            RangeWeapon.transform.SetLocalPositionAndRotation(
                RangeWeapon.WeaponPositionWhenPicked, Quaternion.Euler(RangeWeapon.WeaponRotationWhenPicked));

            (UnitVisuals as PlayerVisuals).EquipPickableRangeWeapon(rangeWeapon);
            RangeWeapon.Equipped(this);

            // For UI todo switch to rangeweapon w/out enum
            if (Enum.TryParse(RangeWeapon.GetType().Name, true, out RangeWeaponEnum weapon))
            {
                OnPlayerChangedWeapon?.Invoke(weapon);
            }
            else Debug.LogWarning($"Player equipped weapon not in enum {typeof(RangeWeaponEnum)}");
        }

        public void EquipPickableRangeWeapon(RangeWeapon rweapon)
        {
            if (PickableWeapon == null) return;

            _defaultPlayerWeapon.gameObject.SetActive(false);
            EquipRangeWeapon(rweapon);
            // Throw away empty gun
            RangeWeapon.OnBulletsEnded += UnequipPickableWeapon;
            
            RangeWeapon.transform.parent = WeaponSlot;            
            PickableWeapon = null;
            ((PlayerInput)UnitInput).SetAnimatorController((UnityEditor.Animations.AnimatorController)RangeWeapon.AnimController);
        }

        public void UnequipPickableWeapon()
        {
            Destroy(RangeWeapon.gameObject);
            _defaultPlayerWeapon.gameObject.SetActive(true);
            EquipRangeWeapon(_defaultPlayerWeapon);
        }
        #endregion


        #region MeleeAttack
        [SerializeField, Space]
        private MeleeWeapon _meleeWeapon;
        public MeleeWeapon MeleeWeapon { get => _meleeWeapon; private set => _meleeWeapon = value; } 

        [SerializeField]
        private float _meleeAttackCooldown = 5f;
        public float MeleeAttackCooldown { get => _meleeAttackCooldown; private set => _meleeAttackCooldown = value; }
        public bool CanHit {get; set;} = true;
        [SerializeField]
        private float _parryInvulTime = 2f;
        public bool Parried = false;

        
        public void MeleeAttack()
        {
            (if CanHit) (UnitVisuals as IMeleeAttack).MeleeAttack();
            StartCoroutine(MeleeAttackCD());
        }
        private IEnumerator MeleeAttackCD()
        {
            CanHit = false;
            yield return new WaitForSeconds(MeleeAttackCooldown);
            CanHit = true;
        }
        public void InMeleeAttack(bool isInMeleeAttack)
        {
            Unit.CanMove = isInMeleeAttack;
            Unit.CanShoot = isInMeleeAttack;
        }

        
        protected void MeleeWeaponBindings()
        {
            MeleeWeapon.OnParry += () => StartCoroutine(Parry());
        }
        private IEnumerator Parry()
        {
            Parried = true;
            (UnitVisuals as PlayerVisuals).Parry();
            yield return new WaitForSeconds(_parryInvulTime);
            Parried = false;            
        }
        #endregion

        public event SimpleHandle OnPlayerDied();
    }