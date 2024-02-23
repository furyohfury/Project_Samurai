using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samurai
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField]
        public LinkedList<Enemy> EnemyList {get; private set; } = new(); 

        public void AddEnemyToPool(Enemy enemy)
        {
            enemy.transform.parent = this.transform;
            EnemyList.AddLast(enemy);            
        }
        public void RemoveEnemyFromPool(Enemy enemy)
        {
#if !UNITY_EDITOR
            enemy.transform.parent = null;
#endif
            EnemyList.Remove(enemy);
        }
    }
}