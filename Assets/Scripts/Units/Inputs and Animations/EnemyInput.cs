using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Samurai
{
    [RequireComponent(typeof(Enemy), typeof(NPCAI))]
    public class EnemyInput : UnitInput
    {
        [Inject]
        protected Player Player;


        public NavMeshAgent Agent { get; private set; }        
        protected NPCAI NPCAI;
        protected CapsuleCollider _collider;


        #region UnityMethods
        protected override void Awake()
        {
            base.Awake();
            if (Agent == null) Agent = GetComponent<NavMeshAgent>();
            if (NPCAI == null) NPCAI = GetComponent<NPCAI>();
            _collider = GetComponent<CapsuleCollider>();

        }
        protected override void Start()
        {
            base.Start();

            // Setting speed of agent according to unit ms
            Agent.speed = Unit.GetUnitStats().MoveSpeed;
        }
        protected override void Update()
        {

            base.Update();
        }
        protected override void FixedUpdate()
        {
            if (CanMove)
            {
                Agent.destination = NPCAI.Target;
                MoveDirection = Vector3.ClampMagnitude(Agent.velocity, 1);
            }
            else MoveDirection = this.transform.position;
            base.FixedUpdate();
        }
        protected void OnValidate()
        {

        }
        #endregion

        public override void UnitInputDie()
        {
            base.UnitInputDie();
            GetComponent<NPCAI>().enabled = false;
            GetComponent<Enemy>().enabled = false;
            CanMove = false;
            // Agent.destination = this.transform.position;
            Agent.enabled = false;
            _collider.enabled = false;
            this.enabled = false;
        }

        public void SetAgentTarget(Vector3 target)
        {
            Agent.destination = target;
        }
    }
}