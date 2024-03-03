using UnityEngine;
using Zenject;
namespace Samurai
{
    public class ShotgunProjectile : Projectile
    {
        [Inject]
        public void Construct(ProjectileManager pmanager)
        {
            ProjectileManager = pmanager;
        }
        public class Factory : PlaceholderFactory<ShotgunProjectile> { }
    }
}