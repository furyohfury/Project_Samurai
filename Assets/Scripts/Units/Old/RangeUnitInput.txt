using DG.Tweening;
using UnityEngine;
namespace Samurai
{
    public class RangeUnitInput : EnemyInput, IAttackRange, IInputRange
    {
        [SerializeField]
        private RangeWeapon _rangeWeapon;
        public RangeWeapon RangeWeapon { get => _rangeWeapon; protected set => _rangeWeapon = value; }
        public bool CanShoot { get; protected set; } = true;

        #region UnityMethods
        protected override void Awake()
        {
            base.Awake();
            RangeWeapon = GetComponentInChildren<RangeWeapon>();
        }
        private void OnEnable()
        {
            NPCAI.OnAttack += Shoot;
        }
        protected override void Start()
        {
            base.Start();
            if (RangeWeapon != null) RangeWeapon.transform.SetLocalPositionAndRotation(
                RangeWeapon.WeaponPositionWhenPicked, Quaternion.Euler(RangeWeapon.WeaponRotationWhenPicked));
            else Debug.LogWarning($"{gameObject.name} didnt find it's range weapon");
        }
        private void OnDisable()
        {
            NPCAI.OnAttack -= Shoot;
        }
        #endregion


        public void Shoot()
        {

            this.transform.LookAt(new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z));
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