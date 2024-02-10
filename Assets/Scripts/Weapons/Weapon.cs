using UnityEditor.Animations;
using UnityEngine;
namespace Samurai
{
    [RequireComponent(typeof(Unit))]
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField]
        protected int _numberOfBulletsForPlayer;
        public int NumberOfBulletsForPlayer { get => _numberOfBulletsForPlayer; protected set => _numberOfBulletsForPlayer = value; }

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

        public abstract Vector3 WeaponPositionWhenPicked { get;}
        public abstract Vector3 WeaponRotationWhenPicked { get;}

        public Unit Owner { get; protected set; }
        
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

        protected virtual void CheckIfEmpty()
        {
            if (Owner as Player != null)
            {
                NumberOfBulletsForPlayer -= 1;
                if (NumberOfBulletsForPlayer <= 0)
                {
                    OnBulletsEnded?.Invoke();
                }
            }
        }

        public SimpleHandle OnBulletsEnded;
    }
}