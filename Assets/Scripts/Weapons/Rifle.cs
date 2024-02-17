using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Samurai
{
    public class Rifle : RangeWeapon
    {
        [SerializeField, Space]
        private int _burstNumberOfShells = 5;
        [SerializeField, Range(0, 10f)]
        private float _burstShellAngleSpread = 5;
        [SerializeField, Space]
        private float _burstDelayBetweenShots = 0.05f;

        public override Vector3 WeaponPositionWhenPicked => new Vector3(0, 0, 0);

        public override Vector3 WeaponRotationWhenPicked => new Vector3(0, 90, 180);

        public override void Shoot()
        {
            var proj = Instantiate(WeaponProjectilePrefab);
            proj.GetComponent<Projectile>().SetProjectileStatsOnShoot(Owner);
            proj.transform.position = this.transform.position + this.transform.forward * 0.1f;
            proj.transform.eulerAngles = new Vector3(0, Owner.transform.eulerAngles.y, 0);
            CheckIfEmpty();
            SetShootingDelay();
        }
        public void ShootBurst()
        {
            StartCoroutine(ShootBurstCor());
        }
        private IEnumerator ShootBurstCor()
        {            
            SetShootingDelay();
            for (var i = 0; i < _burstNumberOfShells; i++)
            {
                var proj = Instantiate(WeaponProjectilePrefab);
                proj.GetComponent<Projectile>().SetProjectileStatsOnShoot(Owner);
                proj.transform.position = this.transform.position + this.transform.forward * 0.1f;
                proj.transform.eulerAngles = new Vector3(
                    0, Owner.transform.eulerAngles.y + Random.Range(-_burstShellAngleSpread, _burstShellAngleSpread), 0);
                yield return new WaitForSeconds(_burstDelayBetweenShots);
            }
        }
    }
}