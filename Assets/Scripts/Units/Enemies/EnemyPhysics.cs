using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Samurai
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyPhysics : UnitPhysics
    {
        [Inject]
        protected CapsuleCollider EnemyHitbox;
        [Inject]
        protected NavMeshAgent Agent;

        #region UnityMethods
        protected void OnEnable()
        {
            EnemyHitbox.enabled = true;
            Agent.enabled = true;
        }
        protected void OnDisable()
        {
            EnemyHitbox.enabled = false;
            Agent.enabled = false;
        }
        #endregion


        protected override void Bindings()
        {
            base.Bindings();
            // EnemyHitbox = GetComponent<CapsuleCollider>();
            // Agent = GetComponent<NavMeshAgent>();
            SetAgentSpeed(Unit.GetUnitStats().MoveSpeed);
        }
        public void SetAgentSpeed(float speed)
        {
            Agent.speed = speed;
        }


        #region Movement        
        public override void Movement(Vector3 direction)
        {
            if (!Unit.CanMove) return;
            if (direction == Vector3.zero) Agent.destination = Agent.transform.position;
            else Agent.destination = direction;
        }
        #endregion
    }
}