using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class Projectile : ColorObject
    {
        protected ProjectileManager ProjectileManager;
        [SerializeField]
        protected Unit _owner;
        public Unit Owner { get => _owner; protected set => _owner = value; }

        [SerializeField]
        protected ProjectileStatsStruct ProjectileStats;
        public ProjectileStatsStruct GetProjectileStats() => ProjectileStats;



        #region UnityMethods
        protected virtual void OnEnable()
        {
            ProjectileManager.ProjectileList.Add(this);
        }
        protected override void Start()
        {
            base.Start();
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MeleeWeapon weapon) && !(Owner is Enemy == weapon.Owner is Enemy))
            {
                Destroy(this.gameObject);
            }
        }
        protected virtual void OnDisable()
        {
            ProjectileManager.ProjectileList.Remove(this);
        }
#endregion

        public void SetProjectileStatsOnShoot(Unit owner)
        {
            Owner = owner;
            ProjectileStats = (owner as IRangeWeapon).RangeWeapon.GetProjectileStats();
            transform.localScale *= ProjectileStats.ProjectileScale;
            ChangeColor(owner.CurrentColor);
        }
        
    }
}