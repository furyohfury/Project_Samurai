using MoreMountains.Tools;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public abstract class Enemy : Unit
    {
        [SerializeField, Space]
        protected GameObject[] HealthPacksPrefabs;
        [SerializeField, Range(0, 1f)]
        private float _hpPackDropChance = 0.2f;

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

            if (HealthPacksPrefabs.Length <= 0) Debug.LogWarning($"Enemy {gameObject.name} doesnt have droppable healthpacks");
        }

        #region GetDamaged
        protected override void ChangeHP(int delta)
        {
            base.ChangeHP(delta);
            HPBar.UpdateBar(UnitStats.HP, 0f, UnitStats.MaxHP, true);
        }
        #endregion

        #region Death
        public override void Die()
        {
            base.Die();
            TryToDropHpPack();
        }
        public override void DiscardUnit()
        {
            EnemyPool.RemoveEnemyFromPool(this);
            Destroy(this.gameObject);
        }

        private void TryToDropHpPack()
        {
            if (Random.value > _hpPackDropChance || HealthPacksPrefabs.Length <= 0) return;
            if (Physics.Raycast(transform.position + Vector3.up * 2, Vector3.down, out RaycastHit hit, 20f, Constants.FloorLayer))
            {
                GameObject hpPack = HealthPacksPrefabs[Random.Range(0, HealthPacksPrefabs.Length - 1)];
                Instantiate(hpPack, hit.point + Vector3.up * 0.5f, Quaternion.Euler(Vector3.zero));
            }
        }
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + Vector3.up * 2, Vector3.down);
        }
#endif
        /// <summary>
        /// Abstract
        /// </summary>
        public abstract void Attack();
    }
}