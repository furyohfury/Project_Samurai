using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class RangeEnemy : Enemy, IRangeWeapon, IRangeAttack
    {
        [SerializeField]
        private RangeWeapon _rangeWeapon;
        public RangeWeapon RangeWeapon { get => _rangeWeapon; private set => _rangeWeapon = value; }
        
        [SerializeField]
        private Transform _rangeWeaponSlot;
        public Transform RangeWeaponSlot {get => _rangeWeaponSlot; set => _rangeWeaponSlot = value;}

        [SerializeField, Range(0, 1f)]
        protected float ChanceToDropRangeWeapon = 0.5f;

        public bool CanShoot { get; set; } = true;


        #region UnityMethods
        #endregion

        protected override Bindings()
        {
            base.Bindings();
            if (RangeWeapon == null) RangeWeapon = GetComponentInChildren<RangeWeapon>();
            EquipRangeWeapon(RangeWeapon);
        }

        public override void Attack() => RangeAttack();
        
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
            RangeWeapon.transform.SetLocalPositionAndRotation(
                RangeWeapon.WeaponPositionWhenPicked, Quaternion.Euler(RangeWeapon.WeaponRotationWhenPicked));

            (UnitVisuals as PlayerVisuals).EquipRangeWeapon(RangeWeapon);
            RangeWeapon.Equipped(this);
        }
        #endregion
    

        #region Death
        public override void Die()
        {
            base.Die();
            if (Random.value < ChanceToDropRangeWeapon) OnDroppedWeapon?.Invoke();
        }

        public event SimpleHandle OnDroppedWeapon;
        #endregion
    }
}