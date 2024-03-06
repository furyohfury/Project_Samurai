using System.Collections;
using System;
using System.Linq;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Samurai
{
    public abstract class Unit : MonoBehaviour
    {
        [Inject]
        public UnitVisuals UnitVisuals { get; protected set; }
        [Inject]
        public UnitPhysics UnitPhysics { get; protected set; }
        [Inject]
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

        [SerializeField]
        protected UnitBuffsStruct UnitBuffs;
        public UnitBuffsStruct GetUnitBuffs()
        {
            return UnitBuffs;
        }

        public bool CanMove { get; set; } = true;

        public MMHealthBar HPBar {get; protected set;}


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
            // UnitVisuals = GetComponent<UnitVisuals>();
            // UnitPhysics = GetComponent<UnitPhysics>();
            // UnitInput = GetComponent<UnitInput>();
            if (UnitStats.HP <= 0 || UnitStats.MaxHP <= 0 || UnitStats.MoveSpeed <= 0) Debug.LogError($"Unit {gameObject.name} has wrong UnitStats");            
            
            // if (TryGetComponent(out MMHealthBar hpbar)) HPBar = hpbar;
            // else Debug.LogError($"No hpbar on {gameObject.name}");
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
            if (UnitStats.HP + delta > UnitStats.MaxHP) UnitStats.HP = UnitStats.MaxHP;
            else UnitStats.HP += delta;

            // For hpbar
            HPBar.UpdateBar(UnitStats.HP, 0f, UnitStats.MaxHP, true);
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
        public abstract void DiscardUnit();
        #endregion

        #region Buffs
        public void ApplyBuff(UnitBuffsStruct buffs)
        {
            UnitStats.MaxHP += buffs.HPBuff;
            UnitStats.MoveSpeed += buffs.MoveSpeedBuff;

            IRangeWeapon RangeUnit = this as IRangeWeapon;
            RangeUnit?.RangeWeapon.ApplyBuff(new ProjectileStatsStruct { Damage = UnitBuffs.RangeWeaponDamageBuff});

            IMeleeWeapon MeleeUnit = this as IMeleeWeapon;
            MeleeUnit?.MeleeWeapon.ApplyBuff(buffs.MeleeWeaponDamageBuff);

            if (MeleeUnit != null) MeleeUnit.MeleeAttackCooldown += buffs.MeleeAttackCDBuff;

            // HUH?
            var unitBuffFields = UnitBuffs.GetType().GetFields();
            var newBuffFields = buffs.GetType().GetFields();
            object box = UnitBuffs;

            foreach(var newBuffField in newBuffFields)
            {
                if (newBuffField.FieldType == typeof(int))
                {
                    var corrUnitBuffField = unitBuffFields.Single((f) => f.Name == newBuffField.Name);
                    corrUnitBuffField.SetValue(box, (int) corrUnitBuffField.GetValue(UnitBuffs) + (int) newBuffField.GetValue(buffs));
                }
                else if (newBuffField.FieldType == typeof(float))
                {
                    var corrUnitBuffField = unitBuffFields.Single((f) => f.Name == newBuffField.Name);
                    corrUnitBuffField.SetValue(box, (float)corrUnitBuffField.GetValue(UnitBuffs) + (float)newBuffField.GetValue(buffs));
                }
            }
            UnitBuffs = (UnitBuffsStruct)box;




            /* foreach (var field in unitBuffFields)
            {
                if (field.GetType() == typeof(int))
                {
                    field.SetValue(this, (int)field.GetValue(this) + (int) (newBuffFields.Single((newbuff) => newbuff.Name == field.Name).GetValue(this)));
                }
                else if (field.GetType() == typeof(float))
                {
                    field.SetValue(this, (float)field.GetValue(this) + (float)(newBuffFields.Single((newbuff) => newbuff.Name == field.Name).GetValue(this)));
                }
                
            } */
        }
        #endregion

        public event SimpleHandle OnUnitHealthChanged;
        
    }
}