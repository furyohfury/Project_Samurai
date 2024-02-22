using System.Collections;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Samurai
{
    public abstract class Unit : MonoBehaviour
    {
        public UnitVisuals UnitVisuals { get; protected set; }
        public UnitPhysics UnitPhysics { get; protected set; }
        public UnitInput UnitInput { get; protected set; }

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

        public bool CanMove { get; set; } = true;


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
            UnitPhysics = GetComponent<UnitPhysics>();
            UnitInput = GetComponent<UnitInput>();
        }

        #region Damaged
        public void GetDamagedByProjectile(Projectile proj)
        {
            // if ((proj.CurrentColor != this.CurrentColor) && (this.GetType() != proj.Owner.GetType()) && (this as Enemy == null || proj.Owner as Enemy == null))
            if ((proj.CurrentColor != this.CurrentColor) && !(this is Enemy == proj.Owner is Enemy))
            {
                UnitVisuals.GetDamagedByProjectile();

                ChangeHP(-proj.GetProjectileStats().Damage);
                var defProj = proj as DefaultPlayerWeaponProjectile;
                if (defProj != null) DefPlayerGunPool.Pool.Release(defProj);
                else Destroy(proj.gameObject);
            }
        }

        /// <summary>
        /// Rewrite for parrying units w/out base
        /// </summary>
        /// <param name="weapon"></param>
        public virtual void GetDamagedByMelee(MeleeWeapon weapon)
        {
            if (!(this is Enemy == weapon.Owner is Enemy))
            {
                UnitVisuals.GetDamagedByMelee();
                ChangeHP(-weapon.Damage);
            }
        }

        protected virtual void ChangeHP(int delta)
        {
            UnitStats.HP += delta;

            // For hpbar
            OnUnitHealthChanged?.Invoke();

            if (UnitStats.HP <= 0)
            {
                Die();
            }
        }
        #endregion


        #region Death
        public virtual void Die()
        {
            UnitPhysics.enabled = false;
            UnitInput.enabled = false;
            UnitVisuals.Die();
            StartCoroutine(DieAwait());
        }
        /// <summary>
        /// Gets processed by managers (or its shit lol)
        /// </summary>
        protected IEnumerator DieAwait()
        {
            yield return new WaitUntil(() => UnitVisuals.DeathAnimationEnded);
            DiscardUnit();
        }
        /// <summary>
        /// Abstract
        /// </summary>
        protected abstract void DiscardUnit();
        #endregion

        public event SimpleHandle OnUnitHealthChanged;
        // public event UnitHandle OnUnitDied;
    }
}