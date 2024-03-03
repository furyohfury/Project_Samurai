using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Samurai
{
    public class DefaultPlayerWeaponProjectile : Projectile
    {             
        
        public void OnReturnedToPool()
        {
            Owner = null;
            transform.localScale = Vector3.one;
        }

        [Inject]
        public void Construct(ProjectileManager pmanager)
        {
            ProjectileManager = pmanager;
        }
        public class Factory : PlaceholderFactory<DefaultPlayerWeaponProjectile> { }
    }
}