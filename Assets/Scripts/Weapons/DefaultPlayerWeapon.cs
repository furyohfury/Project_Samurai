using UnityEngine;
using Zenject;
namespace Samurai
{
    public class DefaultPlayerWeapon : Weapon
    {        
        [Inject]
        private readonly DefaultPlayerGunPool _pool;

        protected override void Start()
        {
            base.Start();
            if (_pool == null) Debug.LogError($"Pool not found by {this.GetType()} component on {gameObject}");
        }
        public override void Shoot()
        {            
            Projectile proj = _pool.Pool.Get();
            proj.SetProjectileStatsOnShoot(Owner);
            proj.transform.position = this.transform.position + this.transform.forward * 0.1f;
            proj.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);

        }
    }
}