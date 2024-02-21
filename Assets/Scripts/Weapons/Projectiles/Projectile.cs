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

        [SerializeField]
        protected ProjectileStatsStruct ProjectileStats;
        public ProjectileStatsStruct GetProjectileStats() => ProjectileStats;        
        public void SetProjectileStatsOnShoot(Unit owner)
        {
            Owner = owner;
            ProjectileStats = (owner as IRangeWeapon).RangeWeapon.GetProjectileStats();
            transform.localScale *= ProjectileStats.ProjectileScale;
            ChangeCurrentColor(owner.CurrentColor);
        }        


        protected virtual void OnEnable()
        {
            if (ProjectileManager.Instance != null) ProjectileManager.Instance.ProjectileList.Add(this);
            else Debug.LogWarning("Projectile didnt find ProjectileManager");
        }
        protected override void Start()
        {
            base.Start();
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MeleeWeapon weapon) && weapon.Owner.GetType() == typeof(Player))
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