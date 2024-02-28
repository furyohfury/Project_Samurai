using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Samurai
{
    public class Rifle : RangeWeapon, IRangePickableWeapon
    {
        [SerializeField, Space]
        private int _burstNumberOfShells = 5;
        [SerializeField, Range(0, 10f)]
        private float _burstShellAngleSpread = 5;
        [SerializeField, Space]
        private float _burstDelayBetweenShots = 0.05f;

        [SerializeField]
        private MMF_Player _burstShootFeedback;

        // public override Vector3 WeaponPositionWhenPicked => new Vector3(0.01, 0, 0.03f);
        public override Vector3 WeaponPositionWhenPicked => new Vector3(0.1f , 0, 0.03f);

        // public override Vector3 WeaponRotationWhenPicked => new Vector3(0, 60, 180);
        public override Vector3 WeaponRotationWhenPicked => new Vector3(-11, 90, 90);

        [SerializeField, Space]
        private MMF_Player _glowingFeedback;
        public MMF_Player GlowingFeedback { get => _glowingFeedback; }

        public override void RangeAttack()
        {
            if (!CanShoot) return;
            if (Owner is Player) RangeAttackSingle();
            else if (Owner is Enemy) RangeAttackBurst();
        }

        private void RangeAttackSingle()
        {
            var proj = Instantiate(WeaponProjectilePrefab);
            proj.GetComponent<Projectile>().SetProjectileStatsOnShoot(Owner);
            proj.transform.position = this.transform.position + this.transform.forward * 0.1f;
            proj.transform.eulerAngles = new Vector3(0, Owner.transform.eulerAngles.y, 0);

            
            ShootingFeedbacks?.PlayFeedbacks();
            SetShootingDelay();
            CheckIfEmpty();
        }

        private void RangeAttackBurst()
        {
            if (!CanShoot) return;
            StartCoroutine(ShootBurstCor());            
            _burstShootFeedback?.PlayFeedbacks();            
        }
        private IEnumerator ShootBurstCor()
        {
            SetShootingDelay();
            for (var i = 0; i < _burstNumberOfShells; i++)
            {
                var proj = Instantiate(WeaponProjectilePrefab);
                if (Owner != null) proj.GetComponent<Projectile>().SetProjectileStatsOnShoot(Owner);
                proj.transform.position = this.transform.position + this.transform.forward * 0.1f;
                proj.transform.eulerAngles = new Vector3(
                    0, Owner.transform.eulerAngles.y + Random.Range(-_burstShellAngleSpread, _burstShellAngleSpread), 0);
                yield return new WaitForSeconds(_burstDelayBetweenShots);
            }
            
            CheckIfEmpty();
        }
    }
}