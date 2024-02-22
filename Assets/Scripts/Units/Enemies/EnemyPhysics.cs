using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Samurai
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyPhysics : UnitPhysics
    {
        protected CapsuleCollider EnemyHitbox;
        protected NavMeshAgent Agent;

        #region UnityMethods
        protected void OnEnable()
        {
            EnemyHitbox.enabled = true;
        }
        protected void OnDisable()
        {
            EnemyHitbox.enabled = false;
        }
        #endregion


        protected override void Bindings()
        {
            base.Bindings();
            EnemyHitbox = GetComponent<CapsuleCollider>();
            Agent = GetComponent<NavMeshAgent>();
            Agent.speed = Unit.GetUnitStats().MoveSpeed;
        }

        #region Movement        
        public override void Movement(Vector3 direction)
        {
            if (!Unit.CanMove) return;
            Agent.destination = direction;
        }
        #endregion
    }
}