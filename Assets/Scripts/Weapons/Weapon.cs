using UnityEngine
namespace Samurai
{
    public abstract class Weapon : MonoBehaviour
    {
        // Gettable        
        [SerializeField]
        protected int Damage;
        [SerializeField]
        protected float ProjectileSpeed;        
        // Ungettable
        [SerializeField]
        protected float AttackSpeed;
        [SerializeField]
        protected GameObject WeaponProjectile;
        protected Unit Owner;
        
        #region Unity_Methods
        protected virtual void Start()
        {
            Owner = GetComponentInParent<Unit>();
        }
        #endregion
        public abstract void Shoot(){}
    }
}