using System.Collections;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Samurai
{
    public abstract class Unit : MonoBehaviour
    {
        protected UnitVisuals UnitVisuals;

        [Inject]
        protected DefaultPlayerGunPool DefPlayerGunPool;

        public PhaseColor CurrentColor { get; protected set; } = PhaseColor.Default;
        public void SetCurrentColor(PhaseColor color) => CurrentColor = color;

        [SerializeField]
        protected UnitStatsStruct UnitStats;
        public UnitStatsStruct GetUnitStats()
        {
            return UnitStats;
        }

        public bool CanMove {get; set;} = true;


        #region UnityMethods
        protected virtual void Awake()
        {
            Bindings();
        }
        #endregion

        /// <summary>
        /// To Awake
        /// </summary>
        protected virtual void Bindings()
        {
            UnitVisuals = GetComponent<UnitVisuals>();
        }

        #region Damaged
        public void GetDamagedByProjectile(Projectile proj)
        {
            if ((proj.CurrentColor != this.CurrentColor) && (this.GetType() != proj.Owner.GetType()) && (this as Enemy == null || proj.Owner as Enemy == null))
            {
                UnitVisuals.GetDamagedByProjectile();                

                ChangeHP(-proj.GetProjectileStats().Damage);

                var defProj = proj as DefaultPlayerWeaponProjectile;
                if (defProj != null) DefPlayerGunPool.Pool.Release(defProj);
                else Destroy(proj.gameObject);
            }
        }

        /// <summary>
        /// Rewrite for parrying units
        /// </summary>
        /// <param name="weapon"></param>
        public virtual void GetDamagedByMelee(MeleeWeapon weapon)
        {
            if ((this.GetType() != weapon.Owner.GetType()) && (this as Enemy == null || weapon.Owner as Enemy == null))
            {
                UnitVisuals.GetDamagedByMelee();
                ChangeHP(-weapon.Damage);
            }
        }

        public virtual void ChangeHP(int delta)
        {
            UnitStats.HP += delta;

            // For hpbar
            OnUnitHealthChanged?.Invoke();

            if (UnitStats.HP <= 0)
            {
                UnitVisuals.Die();
            }
        }
        #endregion


        #region Death
        public void Die()
        {
            UnitPhysics.enabled = false;
            UnitInput.enabled = false;
            UnitVisuals.Die();
            StartCoroutine(DieAwait());
        }
        /// <summary>
        /// Gets processed by managers
        /// </summary>
        protected IEnumerator DieAwait()
        {
            yield return new WaitUntil(() => UnitVisuals.DeathAnimationEnded);
            OnUnitDied?.Invoke(this);
        }
        #endregion

        public event SimpleHandle OnUnitHealthChanged;
        public event UnitHandle OnUnitDied;
    }
}