using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Samurai
{
    public class DefaultPlayerWeaponProjectile : Projectile
    {             
        
        public void OnReturnedToPool()
        {
            Owner = null;
            ProjectileStats.ProjectileSpeed = 0;
            transform.localScale = Vector3.one;
        }        
    }
}