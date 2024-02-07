using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Samurai
{
    public class Projectile : ColorObject
    {
        public Unit Owner {get; private set;}
        public float MoveSpeed {get; private set;}
        public int Damage {get; private set;}


        
        public void SetProjectileOnShoot(Unit owner, float ms, int dmg)
        {
            Owner = owner;
            MoveSpeed = ms;
            Damage = dmg;
        }
        public void OnReturnedToPool()
        {
            Owner = null;
            MoveSpeed = 0;
        }
        private void OnEnable()
        {
            ProjectileManager.Instance.ProjectileList.Add(this);
        }
        private void OnDisable()
        {
            ProjectileManager.Instance.ProjectileList.Remove(this);
        }
        private void OnDestroy()
        {
            
        }
    }
}