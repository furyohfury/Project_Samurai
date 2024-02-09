using UnityEditor.Animations;
using UnityEngine;
namespace Samurai
{
    public abstract class Weapon : MonoBehaviour
    {
        // Gettable        
        /* [SerializeField]
        protected int Damage;
        [SerializeField]
        protected float ProjectileSpeed;
        protected float ProjectileScale; */    
        // Ungettable
        [SerializeField]
        protected float AttackSpeed; //todo
        [SerializeField]
        protected GameObject WeaponProjectilePrefab;
        [SerializeField]
        protected bool _isPickable = true;
        public bool IsPickable
        {
            get => _isPickable;
            protected set => _isPickable = value;
        }
        
        protected Unit Owner;
        
        protected ProjectileStatsStruct ProjectileStats;
        public ProjectileStatsStruct GetProjectileStats() => ProjectileStats;

        [SerializeField]
        protected ProjectileStatsStruct PlayerProjectileStats;
        [SerializeField]
        protected ProjectileStatsStruct EnemyProjectileStats;

        [SerializeField]
        protected AnimatorController _animController;
        public AnimatorController AnimController
        { 
            get => _animController;
            protected set => _animController = value;
        }
        
        
        #region Unity_Methods
        protected virtual void Start()
        {
            Equipped(GetComponentInParent<Unit>());
        }
        #endregion
        public abstract void Shoot();
        public virtual void Equipped(Unit owner)
        {
            if (owner == null) return;
            Owner = owner;
            if (owner.GetType() == typeof(Player)) ProjectileStats = PlayerProjectileStats;
            else ProjectileStats = EnemyProjectileStats;
        }
    }
}