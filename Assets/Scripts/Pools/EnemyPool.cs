using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samurai
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField]
        public List<Enemy> EnemyList;

        public void AddEnemyToPool(Enemy enemy)
        {
            if (EnemyList == null) EnemyList = new();
            enemy.transform.parent = this.transform;
            EnemyList.Add(enemy);            
        }
        public void RemoveEnemyFromPool(Enemy enemy)
        {
#if !UNITY_EDITOR
            enemy.transform.parent = null;
#endif
            EnemyList.Remove(enemy);
            if (EnemyList.Count <= 0) OnAllEnemiesDied?.Invoke();
        }

        public event SimpleHandle OnAllEnemiesDied;
    }
}