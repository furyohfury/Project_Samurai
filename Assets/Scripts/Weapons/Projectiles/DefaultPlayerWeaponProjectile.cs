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
            MoveSpeed = 0;
        }        
    }
}