using UnityEngine;
using Zenject;
namespace Samurai
{
    public class DefaultPlayerWeapon : RangeWeapon
    {        
        [Inject]
        private readonly DefaultPlayerGunPool _pool;

        public override Vector3 WeaponPositionWhenPicked => new Vector3(0.12f , -0.06f , 0.01f);

        public override Vector3 WeaponRotationWhenPicked => new Vector3(0, 90, 90);

        protected void Start()
        {
            if (_pool == null) Debug.LogError($"Pool not found by {this.GetType()} component on {gameObject}");
        }
        public override void RangeAttack()
        {
            if (!CanShoot) return;
            Projectile proj = _pool.Pool.Get();
            proj.SetProjectileStatsOnShoot(Owner);
            proj.transform.position = this.transform.position + this.transform.forward * 0.1f;
            proj.transform.eulerAngles = new Vector3(0, Owner.transform.eulerAngles.y, 0);
            ShootingFeedbacks?.PlayFeedbacks();
            SetShootingDelay();
        }

        public class Factory : PlaceholderFactory<DefaultPlayerWeapon> { }
    }
}