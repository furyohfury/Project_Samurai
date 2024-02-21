using UnityEngine;
namespace Samurai
{
    public abstract class UnitPhysics : MonoBehaviour
    {
        protected Unit Unit;
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
            Unit = GetComponent<Unit>();
            UnitVisuals = GetComponent<UnitVisuals>();
        }


        #region Movement
        public virtual void Movement(Vector3 direction)
        {
            if (!Unit.CanMove) return;
        }
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