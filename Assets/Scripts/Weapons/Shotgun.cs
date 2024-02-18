using UnityEngine;
namespace Samurai
{
    public class Shotgun : RangeWeapon
    {
        [SerializeField]
        private int _numberOfShells;
        [SerializeField, Range(0, 15f)]
        private float _shellAngleSpread;

        public override Vector3 WeaponPositionWhenPicked { get => new Vector3(0.1f, -0.1f, 0); }
        public override Vector3 WeaponRotationWhenPicked { get => new Vector3(145, -90, 0); }

        
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
            ShootingFeedbacks?.PlayFeedbacks();
            CheckIfEmpty();
            SetShootingDelay();
        }
    }
}