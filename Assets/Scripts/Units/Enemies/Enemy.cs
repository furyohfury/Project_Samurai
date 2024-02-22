using System.Collections;
using UnityEngine;

namespace Samurai
{
    public abstract class Enemy : Unit
    {
        [Inject]
        private EnemyPool EnemyPool;

        [SerializeField]
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
            HPBar ??= GetComponent<MMHealthBar>();
            HPBar.UpdateBar(UnitStats.HP, 0f, UnitStats.MaxHP, true);
        }

        #region GetDamaged
        protected virtual void ChangeHP(int delta)
        {
            base.ChangeHP(delta);
            HPBar.UpdateBar(UnitStats.HP + delta, 0f, UnitStats.MaxHP, true);
        }
        #endregion
        
        public abstract void Attack();        
    }
}