using UnityEngine;
using Zenject;
namespace Samurai
{
    public abstract class UnitPhysics : MonoBehaviour
    {
        [Inject]
        protected Unit Unit;
        [Inject]
        protected UnitVisuals UnitVisuals;



        #region UnityMethods
        protected virtual void Awake()
        {
            Bindings();
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Projectile proj)) GetDamagedByProjectile(proj);
            else if (other.TryGetComponent(out MeleeWeapon mweapon)) GetDamagedByMelee(mweapon);
        }
        #endregion

        protected virtual void Bindings()
        {
            // Unit = GetComponent<Unit>();
            // UnitVisuals = GetComponent<UnitVisuals>();
        }


        #region Movement
        public abstract void Movement(Vector3 direction);
        #endregion


        #region Damaged
        public void GetDamagedByProjectile(Projectile proj)
        {
            Unit.GetDamagedByProjectile(proj);
        }
        public virtual void GetDamagedByMelee(MeleeWeapon mweapon)
        {
            Unit.GetDamagedByMelee(mweapon);
        }
        #endregion
    }
}