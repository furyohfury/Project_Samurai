using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samurai
{
    public class EnemyPool : MonoBehaviour
    {
        public List<Enemy> EnemyList;
        private void Start()
        {
            if (EnemyList.Count <= 0)
            {
                Debug.LogError($"EnemyPool in {transform.root.name} didn't have any enemies");
                EnemyList.AddRange(GetComponentsInChildren<Enemy>());
            }
        }

        public void AddEnemyToPool(Enemy enemy)
        {
            if (EnemyList.Count <= 0) EnemyList = new();
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