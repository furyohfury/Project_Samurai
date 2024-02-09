using UnityEngine
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
        
        protected Unit Owner;
        
        protected ProjectileStatsStruct ProjectileStats;
        public ProjectileStatsStruct GetProjectileStats() => ProjectileStats;

        [SerializeField]
        protected ProjectileStatsStruct PlayerProjectileStats;
        [SerializeField]
        protected ProjectileStatsStruct EnemyProjectileStats;
        
        
        #region Unity_Methods
        protected virtual void Start()
        {
            Owner = GetComponentInParent<Unit>();
        }
        #endregion
        /* public virtual void ChangeOwner(Unit owner)
        {
            Owner = owner;
            ProjectileSpeed = owner.GetUnitStats().ProjectileSpeed;
            Damage = owner.GetUnitStats().Damage;
            ProjectileScale = owner.GetUnitStats().ProjectileScale;
        } */
        public abstract void Shoot(){}
        public vurtial void Equipped(Unit owner)
        {
            Owner = owner;
            if (owner.GetType() == typeof(Player)) ProjectileStats = PlayerProjectileStats;
            else ProjectileStats = EnemyProjectileStats;
        }
    }
}