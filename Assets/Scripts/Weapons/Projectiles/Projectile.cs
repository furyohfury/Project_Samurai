using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Samurai
{
    public class Projectile : ColorObject
    {
        public Unit Owner {get; protected set;}
        protected ProjectileStatsStruct ProjectileStats;
        public ProjectileStatsStruct GetProjectileStats() => ProjectileStats;
        
        public void SetProjectileStatsOnShoot(Unit owner)
        {
            Owner = owner;
            ProjStats = owner.UnitWeapon.GetProjectileStats();
            ChangeColor(owner.CurrentColor);
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