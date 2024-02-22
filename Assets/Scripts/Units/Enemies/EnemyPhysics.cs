using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class EnemyPhysics : UnitPhysics
    {
        protected CapsuleCollider EnemyHitbox;

        #region UnityMethods
        protected override OnEnable()
        {
            EnemyHitbox.enabled = true;
        }
        protected override OnDisable()
        {
            EnemyHitbox.enabled = false;
        }
        #endregion


        protected override void Bindings()
        {
            EnemyHitbox = GetComponent<CapsuleCollider>();
        }

        #region Movement
        protected NavMeshAgent Agent;
        protected override void Movement(Vector3 direction)
        {
            if (CanMove) Agent.destination = direction;
        }
        #endregion
    }
}