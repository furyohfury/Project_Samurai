using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samurai
{
    public class EnemyPool : MonoBehaviour
    {
        public LinkedList<Enemy> EnemyList = new();
    }
}