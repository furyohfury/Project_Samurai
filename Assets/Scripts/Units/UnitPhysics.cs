using UnityEngine;
namespace Samurai
{
    public abstract class UnitPhysics : MonoBehaviour
    {
        protected Unit Unit;
        protected UnitVisuals UnitVisuals;

        #region Movement
        public abstract void Movement(Vector3 direction);
        #endregion


        #region Damaged
        public void GetDamagedByProjectile(Projectile proj) //todo add to ontriggerenter
        {
            Unit.GetDamagedByProjectile(proj);
            UnitVisuals.GetDamagedByProjectile();
        }
        #endregion
    }
}