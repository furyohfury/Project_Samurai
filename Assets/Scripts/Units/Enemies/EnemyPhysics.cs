using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class EnemyPhysics : UnitPhysics
    {
        #region Movement
        protected NavMeshAgent Agent;
        protected override void Movement(Vector3 direction)
        {
            if (CanMove) Agent.destination = direction;
        }
        #endregion
    }
}