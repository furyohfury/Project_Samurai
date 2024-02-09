using UnityEngine;
namespace Samurai
{
    public class Shotgun : PickableWeapon
    {
        public void Shoot()
        {       
            Projectile proj;     
            if (Owner.GetType() == typeof(Player))
            {

            } else 
            {

            }

            proj.SetProjectileStatsOnShoot(Owner, ProjectileSpeed, UnitStats.Damage, Owner.CurrentColor);
            proj.transform.SetPositionAndRotation(this.transform.position + this.transfrom.forward * 0.1f, this.transform.rotation);
        }
    }
}