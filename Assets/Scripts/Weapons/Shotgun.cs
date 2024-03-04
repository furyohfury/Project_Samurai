using MoreMountains.Feedbacks;
using UnityEngine;
using Zenject;
namespace Samurai
{
    public class Shotgun : RangeWeapon, IRangePickableWeapon
    {
        [Inject]
        private ShotgunProjectile.Factory _factory;

        [SerializeField]
        private int _numberOfShells;
        [SerializeField, Range(0, 15f)]
        private float _shellAngleSpread;

        public override Vector3 WeaponPositionWhenPicked { get => new Vector3(0.08f, -0.02f, 0); }
        // public override Vector3 WeaponRotationWhenPicked { get => new Vector3(145, -90, 0); }
        public override Vector3 WeaponRotationWhenPicked { get => new Vector3(10.2f, 62, 90); }

        [SerializeField, Space]
        private MMF_Player _glowingFeedback;
        public MMF_Player GlowingFeedback { get => _glowingFeedback; }

        public override void RangeAttack()
        {
            if (!CanShoot || Owner == null) return;
            SetShootingDelay();
            for (var i = 0; i < _numberOfShells; i++)
            {
                var proj = _factory.Create()
                proj.SetProjectileStatsOnShoot(Owner);
                proj.transform.position = this.transform.position + this.transform.forward * 0.1f;
                proj.transform.eulerAngles = new Vector3(
                    0, Owner.transform.eulerAngles.y + Random.Range(-_shellAngleSpread, _shellAngleSpread), 0);
            }
            ShootingFeedbacks?.PlayFeedbacks();            
            
            CheckIfEmpty();
        }
    }
}