using UnityEngine;
namespace Samurai
{
    public class Shotgun : Weapon
    {
        [SerializeField]
        private int _numberOfShells;
        [SerializeField, Range(0, 10f)]
        private float _shellAngleSpread;
        protected override void Start()
        {
            base.Start();
        }
        public override void Shoot()
        {                  
            for (var i = 0; i < _numberOfShells; i++)
            {
                var proj = Instantiate(WeaponProjectilePrefab);
                proj.GetComponent<Projectile>().SetProjectileStatsOnShoot(Owner);
                proj.transform.position = this.transform.position + this.transform.forward * 0.1f;
                proj.transform.eulerAngles = new Vector3(
                    0, this.transform.eulerAngles.y + Random.Range(-_shellAngleSpread, _shellAngleSpread), 0);
            }            
        }
    }
}