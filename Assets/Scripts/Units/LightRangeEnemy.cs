using UnityEngine;
namespace Samurai
{
    public class LightRangeEnemy : Enemy, IAttackRange
    {   
        [SerializeField, Range(0, 1f)]
        protected float ChanceToDropRangeWeapon;
        [SerializeField]
        private Transform LocationToDropWeapon;


        protected override void Die()
        {
            if (Random.value < ChanceToDropRangeWeapon) DropRangeWeapon();
            base.Die();
        }
        protected void DropRangeWeapon()
        {
            RangeWeapon.transform.position = LocationToDropWeapon.position;
            RangeWeapon.transform.parent = null;
            RangeWeapon.Owner = null;
            OnDroppedWeapon?.Invoke();
        }
        public event SimpleHandle OnDroppedWeapon;
    }
}