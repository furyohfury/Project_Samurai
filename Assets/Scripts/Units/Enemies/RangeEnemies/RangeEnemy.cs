using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class RangeEnemy : Enemy, IRangeWeapon, IRangeAttack
    {
        [SerializeField]
        private RangeWeapon _rangeWeapon;
        public RangeWeapon RangeWeapon { get => _rangeWeapon; private set => _rangeWeapon = value; }
        public bool CanShoot { get; set; } = true;
        [SerializeField]
        private Transform _rangeWeaponSlot;
        public Transform RangeWeaponSlot {get => _rangeWeaponSlot; set => _rangeWeaponSlot = value;}


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
    }
}