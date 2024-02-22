using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class EnemyPhysics : UnitPhysics
    {
        protected CapsuleCollider EnemyHitbox;
        protected NavMeshAgent Agent;

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
            Agent.speed = Unit.GetUnitStats().MoveSpeed;
        }

        #region Movement        
        protected override void Movement(Vector3 direction)
        {
            if (CanMove) Agent.destination = direction;
        }
        #endregion
    }
}