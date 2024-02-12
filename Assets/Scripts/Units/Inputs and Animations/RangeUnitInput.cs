using UnityEngine;
namespace Samurai
{
    public class RangeUnitInput : EnemyInput, IAttackRange, IInputRange
    {
        [SerializeField]
        private RangeWeapon _rangeWeapon;
        public RangeWeapon RangeWeapon { get => _rangeWeapon; protected set => _rangeWeapon = value; }
        public bool CanShoot { get; protected set; } = true;


        protected override void Awake()
        {
            base.Awake();
            RangeWeapon = GetComponentInChildren<RangeWeapon>();
        }
        protected override void Start()
        {
            base.Start();
            if (RangeWeapon != null) RangeWeapon.transform.SetLocalPositionAndRotation(
                RangeWeapon.WeaponPositionWhenPicked, Quaternion.Euler(RangeWeapon.WeaponRotationWhenPicked));
            else Debug.LogWarning($"{gameObject.name} didnt find it's range weapon");
        }
        public void Shoot()
        {
            if ((Unit as IAttackRange).RangeWeapon.CanShoot)
            {
                UnitAnimator.SetTrigger("Shoot");
                (Unit as IAttackRange).Shoot();
            }
        }
        public void OnShootAnimationEnded_UnityEvent()
        {
            
        }

        public void OnShootAnimationStarted_UnityEvent()
        {
            
        }
    }
}