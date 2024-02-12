using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Samurai
{
    public class Rifle : RangeWeapon
    {
        public override Vector3 WeaponPositionWhenPicked => new Vector3(0, 0, 0);

        public override Vector3 WeaponRotationWhenPicked => new Vector3(0, 90, 180);

        public override void Shoot()
        {
            var proj = Instantiate(WeaponProjectilePrefab);
            proj.GetComponent<Projectile>().SetProjectileStatsOnShoot(Owner);
            proj.transform.position = this.transform.position + this.transform.forward * 0.1f;
            proj.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
            CheckIfEmpty();
            SetShootingDelay(ShootingDelay);
        }
    }
}