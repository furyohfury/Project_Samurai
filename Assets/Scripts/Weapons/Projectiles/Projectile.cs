using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Samurai
{
    public class Projectile : ColorObject
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
        protected virtual void OnEnable()
        {
            ProjectileManager.Instance.ProjectileList.Add(this);
        }
        protected virtual void OnDisable()
        {
            ProjectileManager.Instance.ProjectileList.Remove(this);
        }
        protected virtual void OnDestroy()
        {
            
        }
    }
}