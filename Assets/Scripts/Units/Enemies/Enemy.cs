using MoreMountains.Tools;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public abstract class Enemy : Unit
    {
        private EnemyPool EnemyPool;

        protected MMHealthBar HPBar;

        #region UnityMethods
        protected void OnEnable()
        {
            HPBar.enabled = true;
        }
        protected void OnDisable()
        {
            
        }
        #endregion


        protected override void Bindings()
        {
            base.Bindings();
            if (TryGetComponent(out MMHealthBar hpbar))
            {
                HPBar = hpbar;
            }
            else Debug.LogError($"No hpbar on {gameObject.name}");

            EnemyPool = GetComponentInParent<EnemyPool>();
            if (EnemyPool == null) Debug.LogError($"Enemy {gameObject.name} didnt find its EnemyPool");
        }

        #region GetDamaged
        protected override void ChangeHP(int delta)
        {
            base.ChangeHP(delta);
            HPBar.UpdateBar(UnitStats.HP, 0f, UnitStats.MaxHP, true);
        }
        #endregion

        #region Death
        public override void DiscardUnit()
        {
            EnemyPool.RemoveEnemyFromPool(this);
            Destroy(this.gameObject);
        }
        #endregion

        /// <summary>
        /// Abstract
        /// </summary>
        public abstract void Attack();
    }
}