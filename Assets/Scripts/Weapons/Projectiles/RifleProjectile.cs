using System.Collections;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class RifleProjectile : Projectile
    {
        [Inject]
        public void Construct(ProjectileManager pmanager)
        {
            ProjectileManager = pmanager;
        }
        public class Factory : PlaceholderFactory<RifleProjectile> { }
    }
}