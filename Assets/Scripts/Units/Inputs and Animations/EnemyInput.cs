using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Samurai
{
    [RequireComponent(typeof(Enemy), typeof(NPCAI))]
    public class EnemyInput : UnitInput
    {
        public NavMeshAgent Agent {get; private set;}
        [Inject]
        protected Player Player;
        protected NPCAI NPCAI;
        
        protected override void Awake()
        {
            base.Awake();
            Agent = GetComponent<NavMeshAgent>();
            NPCAI = GetComponent<NPCAI>();
        }
        protected override void Update()
        {                        
            MoveDirection = Vector3.ClampMagnitude(Agent.velocity,1);
            base.Update();
        }
        public void SetAgentTarget(Vector3 target)
        {
            Agent.destination = target;
        }
    }
}