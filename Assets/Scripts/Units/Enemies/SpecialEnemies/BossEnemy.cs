using UnityEngine;
namespace Samurai
{
    public class BossEnemy : Enemy, IMeleeAttack, IMeleeWeapon, IRangeAttack, IRangeWeapon
    {
        protected override void Bindings()
        {
            base.Bindings();
            MeleeWeaponBindings();
            if (RangeWeapon == null) RangeWeapon = GetComponentInChildren<RangeWeapon>();
            EquipRangeWeapon(RangeWeapon);
        }

        // For IMeleeWeapon
        #region GetDamaged
        public override void GetDamagedByMelee(MeleeWeapon weapon)
        {
            if (!Parried && !(this is Enemy == weapon.Owner is Enemy))
            {
                UnitVisuals.GetDamagedByMelee();
                ChangeHP(-weapon.Damage);
            }
        }
        #endregion


        // For IMeleeAttack
        #region MeleeAttack
        [SerializeField, Space]
        private MeleeWeapon _meleeWeapon;
        public MeleeWeapon MeleeWeapon { get => _meleeWeapon; private set => _meleeWeapon = value; }

        [SerializeField]
        private float _meleeAttackCooldown = 5f;
        public float MeleeAttackCooldown { get => _meleeAttackCooldown; set => _meleeAttackCooldown = value; }
        public bool CanHit { get; set; } = true;


        public void MeleeAttack()
        {
            if (CanHit)
            {
                (UnitVisuals as IMeleeAttack).MeleeAttack();
                MeleeWeapon.MeleeAttack();

                StartCoroutine(MeleeAttackCD());
            }
        }
        private IEnumerator MeleeAttackCD()
        {
            CanHit = false;
            yield return new WaitForSeconds(MeleeAttackCooldown);
            CanHit = true;
        }
        public void InMeleeAttack(bool isInMeleeAttack)
        {
            CanMove = !isInMeleeAttack;
        }

        // Parry
        [SerializeField]
        private float _parryInvulTime = 2f;
        public bool Parried { get; set; } = false;

        protected void MeleeWeaponBindings()
        {
            MeleeWeapon.OnParry += Parry;
        }
        public void Parry()
        {
            if (!Parried) StartCoroutine(ParryCoroutine());
        }
        private IEnumerator ParryCoroutine()
        {
            Parried = true;
            yield return new WaitForSeconds(_parryInvulTime);
            Parried = false;
        }
        #endregion


        // IRangeWeapon
        #region RangeWeapon
        [SerializeField]
        private RangeWeapon _rangeWeapon;
        public RangeWeapon RangeWeapon { get => _rangeWeapon; private set => _rangeWeapon = value; }

        [SerializeField]
        private Transform _rangeWeaponSlot;
        public Transform RangeWeaponSlot { get => _rangeWeaponSlot; set => _rangeWeaponSlot = value; }

        public bool CanShoot { get; set; } = true;
        #endregion


        // For IRangeAttack
        #region RangeAttack
        public void RangeAttack()
        {
            if (CanShoot && RangeWeapon.CanShoot)
            {
                RangeWeapon.RangeAttack();
                (UnitVisuals as IRangeAttack).RangeAttack();
            }
        }

        public void EquipRangeWeapon(RangeWeapon rWeapon)
        {
            RangeWeapon = rWeapon;
            // RangeWeapon.transform.SetLocalPositionAndRotation(RangeWeapon.WeaponPositionWhenPicked, Quaternion.Euler(RangeWeapon.WeaponRotationWhenPicked));

            RangeWeapon.Equipped(this);
        }
        #endregion
        
    }
}