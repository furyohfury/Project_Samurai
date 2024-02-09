using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Samurai
{
    public class DefaultPlayerWeaponProjectile : Projectile
    {
        public Unit Owner {get; protected set;}
        public float MoveSpeed {get; protected set;}
        public int Damage {get; protected set;}

        
        public void SetProjectileStatsOnShoot(Unit owner, float ms, int dmg, PhaseColor color)
        {
            Owner = owner;
            MoveSpeed = ms;
            Damage = dmg;
            ChangeColor(color);
        }
        public void OnReturnedToPool()
        {
            Owner = null;
            MoveSpeed = 0;
        }        
    }
}