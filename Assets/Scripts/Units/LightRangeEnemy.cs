using UnityEngine;
namespace Samurai
{
    public class LightRangeEnemy : Enemy, IAttackRange
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
            //  RangeWeapon.transform.position = LocationToDropWeapon.position;            
            OnDroppedWeapon?.Invoke();
        }

        public void Shoot()
        {
            RangeWeapon.Shoot();
        }

        public event SimpleHandle OnDroppedWeapon;
    }
}