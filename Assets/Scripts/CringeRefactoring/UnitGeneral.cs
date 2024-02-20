using UnityEngine;
namespace Samurai
{
    public class UnityGeneral : ColorObject
    {
        [SerializeField]
        protected UnitStatsStruct UnitStats;
        public UnitStatsStruct GetUnitStats()
        {
            return UnitStats;
        }

        public void GetDamagedByProjectile(Projectile proj)
        {

        }
        public void GetDamagedByMelee(MeleeWeapon weapon)
        {

        }
        public virtual void ChangeHP (int delta)
        {
            UnitStats.HP += delta;

            // For hpbar
            OnUnitHealthChanged?.Invoke();
        }
        protected virtual void GetDamaged(int damage)
        {
            UnitStats.HP -= damage;

            OnUnitHealthChanged?.Invoke();
            
            if (UnitStats.HP <= 0)
            {
                
            }
        }

        public event SimpleHandle OnUnitHealthChanged;
    }
}