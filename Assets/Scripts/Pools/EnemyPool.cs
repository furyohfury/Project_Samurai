using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samurai
{
    public class EnemyPool : MonoBehaviour
    {
        public LinkedList<Enemy> EnemyList {get; private set; } = new(); 

        public void AddEnemyToPool(Enemy enemy)
        {
            enemy.transform.parent = this.gameObject;
            EnemyList.AddLast(enemy);            
        }
        public void RemoveEnemyFromPool(Enemy enemy)
        {
            enemy.transform.parent = null;
            EnemyList.Remove(enemy);
        }
    }
}