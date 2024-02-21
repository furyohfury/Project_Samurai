using UnityEngine;
using Zenject;
namespace Samurai
{
    public class DefaultPlayerWeapon : RangeWeapon
    {        
        [Inject]
        private readonly DefaultPlayerGunPool _pool;

        public override Vector3 WeaponPositionWhenPicked => Vector3.zero;

        public override Vector3 WeaponRotationWhenPicked => Vector3.zero;

        protected override void Start()
        {
            base.Start();
            if (_pool == null) Debug.LogError($"Pool not found by {this.GetType()} component on {gameObject}");
        }
        public override void RangeAttack()
        {
            Projectile proj = _pool.Pool.Get();
            proj.SetProjectileStatsOnShoot(Owner);
            proj.transform.position = this.transform.position + this.transform.forward * 0.1f;
            proj.transform.eulerAngles = new Vector3(0, Owner.transform.eulerAngles.y, 0);
            ShootingFeedbacks?.PlayFeedbacks();
            SetShootingDelay();
        }
    }
}