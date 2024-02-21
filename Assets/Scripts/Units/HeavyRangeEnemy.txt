using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class HeavyRangeEnemy : Enemy, IAttackRange
    {
        [SerializeField]
        private RangeWeapon _rangeWeapon;
        public RangeWeapon RangeWeapon { get => _rangeWeapon; private set => _rangeWeapon = value; }

        [SerializeField, Range(0, 1f)]
        protected float ChanceToDropRangeWeapon;
        // [SerializeField]
        // private Transform LocationToDropWeapon;
        

        protected override void Awake()
        {
            base.Awake();
            RangeWeapon = GetComponentInChildren<RangeWeapon>();
        }
        public override void Die()
        {
            if (Random.value < ChanceToDropRangeWeapon) DropRangeWeapon();
            base.Die();
        }
        protected void DropRangeWeapon()
        {
            OnDroppedWeapon?.Invoke();
        }
        public void Shoot()
        {
            (RangeWeapon as Rifle).ShootBurst();
        }

        public event SimpleHandle OnDroppedWeapon;
    }
}