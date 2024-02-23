using MoreMountains.Tools;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public abstract class Enemy : Unit
    {
        [Inject]
        private EnemyPool EnemyPool;

        protected MMHealthBar HPBar;

        #region UnityMethods
        protected void OnEnable()
        {
            EnemyPool.AddEnemyToPool(this);
        }
        protected void OnDisable()
        {
            EnemyPool.RemoveEnemyFromPool(this);
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
        }

        #region GetDamaged
        protected override void ChangeHP(int delta)
        {
            base.ChangeHP(delta);
            HPBar.UpdateBar(UnitStats.HP + delta, 0f, UnitStats.MaxHP, true);
        }
        #endregion
        
        #region Death
        protected override void DiscardUnit()
        {
            Destroy(this.gameObject);
        }
        #endregion

        /// <summary>
        /// Abstract
        /// </summary>
        public abstract void Attack();        
    }
}