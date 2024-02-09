using UnityEngine
using Zenject;
namespace Samurai
{
    public class DefaultPlayerWeapon : Weapon
    {        
        [Inject]
        private DefaultGunPlayerPool _pool;

        protected override void Start()
        {
            base.Start();
            if (_pool == null) Debug.LogError($"Pool not found by {this.GetType()} component on {gameObject}");
        }
        public void Shoot()
        {            
            Projectile proj = _pool.Pool.Get();
            proj.SetProjectileStatsOnShoot(Owner, ProjectileSpeed, UnitStats.Damage, Owner.CurrentColor);
            proj.transform.SetPositionAndRotation(this.transform.position + this.transfrom.forward * 0.1f, this.transform.rotation);
        }
    }
}