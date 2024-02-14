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
        #region UnityMethods
        protected override void Awake()
        {
            base.Awake();
            if (Agent == null) Agent = GetComponent<NavMeshAgent>();
            if (NPCAI == null) NPCAI = GetComponent<NPCAI>();
            
        }
        protected override void Start()
        {
            base.Start();
            Agent.speed = Unit.GetUnitStats().MoveSpeed;
        }
        protected override void Update()
        {                        
            
            base.Update();
        }
        protected override void FixedUpdate()
        {
            Agent.destination = NPCAI.Target;
            MoveDirection = Vector3.ClampMagnitude(Agent.velocity, 1);
            base.FixedUpdate();
        }
        protected void OnValidate()
        {
            
        }
        #endregion

        public void SetAgentTarget(Vector3 target)
        {
            Agent.destination = target;
        }
    }
}