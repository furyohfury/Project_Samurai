using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class Minigun : RangeWeapon
    {
        [SerializeField]
        private Animator _minigunAnimator;

        [SerializeField, Space]
        private int _burstNumberOfShells = 20;
        [SerializeField, Range(0, 90f)]
        private float _burstShellAngleSpread = 30;
        [SerializeField, Space]
        private float _burstDelayBetweenShots = 0.02f;

        [SerializeField]
        private MMF_Player _burstShootFeedback;

        // public override Vector3 WeaponPositionWhenPicked => new Vector3(0.01, 0, 0.03f);
        public override Vector3 WeaponPositionWhenPicked => new Vector3(0.1f, 0, 0.03f);

        // public override Vector3 WeaponRotationWhenPicked => new Vector3(0, 60, 180);
        public override Vector3 WeaponRotationWhenPicked => new Vector3(-11, 90, 90);

        public override void RangeAttack()
        {
            if (!CanShoot) return;
            RangeAttackBurst();
        }

        private void RangeAttackBurst()
        {
            if (!CanShoot) return;
            SetShootingDelay();
            StartCoroutine(ShootBurstCor());

            _minigunAnimator.SetBool("RangeAttack", true);

        }
        private IEnumerator ShootBurstCor()
        {
            _burstShootFeedback?.PlayFeedbacks();

            for (var i = 0; i < _burstNumberOfShells; i++)
            {
                var proj = Instantiate(WeaponProjectilePrefab);
                if (Owner != null) proj.GetComponent<Projectile>().SetProjectileStatsOnShoot(Owner);
                proj.transform.position = this.transform.position + this.transform.forward * 0.1f;
                proj.transform.eulerAngles = new Vector3(
                    0, Owner.transform.eulerAngles.y + Random.Range(-_burstShellAngleSpread, _burstShellAngleSpread), 0);
                yield return new WaitForSeconds(_burstDelayBetweenShots);
            }

            _burstShootFeedback?.StopFeedbacks();
            _minigunAnimator.SetBool("RangeAttack", false);

            CheckIfEmpty();
        }
    }
}