using UnityEngine;
using UnityEngine.Pool;
using Zenject;
namespace Samurai
{
    public class Obstacle: ColorObject
    {
        [Inject]
        private DefaultPlayerGunPool _defaultPlayerGunPool;

        protected override void Start()
        {
            base.Start();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Projectile proj) && proj.CurrentColor != this.CurrentColor)
            {
                _defaultPlayerGunPool.Pool.Release(proj);
            }
        }
    }
}