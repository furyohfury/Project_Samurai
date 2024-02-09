using UnityEngine;
namespace Samurai
{
    public class Shotgun : PickableWeapon
    {
        [SerializeField]
        private int _numberOfShells;
        [SerializeField, Range(0, 10f)]
        private float _shellAngleSpread;
        public void Shoot()
        {                  
            for (var i = 0; i < _numberOfShells; i++)
            {
                Instantiate(WeaponProjectilePrefab);
                proj.GetComponent<Projectile>().SetProjectileStatsOnShoot(Owner);
                proj.transform.SetPositionAndRotation(this.transform.position + this.transfrom.forward * 0.1f, this.transform.rotation);
                proj.transform.rotation.eulerAngles.y += Random.Range(0, _shellAngleSpread); // What axis
            }            
        }
    }
}