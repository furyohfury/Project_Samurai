using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class Projectile : ColorObject
    {
        // [Inject]
        // protected ProjectileManager ProjectileManager;
        [SerializeField]
        protected Unit _owner;
        public Unit Owner { get => _owner; protected set => _owner = value; }
        protected ProjectileStatsStruct ProjectileStats;
        public ProjectileStatsStruct GetProjectileStats() => ProjectileStats;        
        public void SetProjectileStatsOnShoot(Unit owner)
        {
            Owner = owner;
            ProjectileStats = owner.UnitWeapon.GetProjectileStats();
            transform.localScale *= ProjectileStats.ProjectileScale;
            ChangeColor(owner.CurrentColor);
        }        


        protected virtual void OnEnable()
        {
            ProjectileManager.Instance.ProjectileList.Add(this);
        }
        protected override void Start()
        {
            base.Start();            
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MeleeWeapon weapon) && weapon.Owner != Owner)
            {
                Destroy(this.gameObject);
            }
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