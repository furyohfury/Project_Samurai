using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class Projectile : ColorObject
    {
        public Unit Owner {get; private set;}
        public float MoveSpeed {get; private set;}
        public int Damage {get; private set;}

        public OnReturnedToPool()
        {
            Owner = null;
            MoveSpeed = 0;
        }
    }
}