using System.Collections;
using UnityEngine;

namespace Samurai
{
    public abstract class EnemyInput : UnitInput
    {
        [Inject]
        protected Player Player;     

        [SerializeField]
        protected AIStateType AIState = AIStateType.Idle;
        [SerializeField]
        protected float LogicUpdateTime;

        /// <summary>
        /// Main property for this component to set
        /// </summary>
        public Vector3 Target { get; protected set; }  

        // Spotting        
        protected bool SpottedPlayer = false;
        [SerializeField, Space]
        protected float PlayerSpotRange;
        protected bool PlayerIsInSpotRange
        {
            get
            {
                Vector3 distanceToPlayer = Player.transform.position - this.transform.position;
                return distanceToPlayer.sqrMagnitude < PlayerSpotRange * PlayerSpotRange;
            }
        }

        //Attacking
        [SerializeField]
        protected float AttackRange;
        protected bool PlayerIsInAttackRange
        {
            get
            {
                Vector3 distanceToPlayer = Player.transform.position - this.transform.position;
                return distanceToPlayer.sqrMagnitude < AttackRange * AttackRange;
            }
        }

        //Patrolling        
        [SerializeField, Space]
        protected float PatrollingIdleDelay;
        [SerializeField]
        protected float PatrollingIdleRandomStep;
        [SerializeField]
        protected Vector3[] PatrollingPoints;
               
        protected Vector3 StartPoint;
        protected int CurrentPatrollingPointIndex;
        protected float ArrivalDistance;

        protected Coroutine PatrollingDelayCoroutine;        
        
        #region UnityMethods
        protected void FixedUpdate()
        {
            UnitVisuals.Movement(Target);
            UnitPhysics.Movement(Target);
        }
        #endregion


        /// <summary>
        /// First method for AIManager
        /// </summary>
        public void StartingIdlePatrolLogic()
        {
            if (PatrollingPoints != null && PatrollingPoints.Length > 1)
            {
                // Level height of patrolling points with NPC
                for (var i = 0; i < PatrollingPoints.Length; i++)
                {
                    PatrollingPoints[i].y = transform.position.y;
                }
                AIState = AIStateType.Patrolling;
                StartPoint = this.transform.position;
            }
            else
            {
                AIState = AIStateType.Idle;
                StartPoint = this.transform.position;
                PatrollingPoints = new Vector3[0];
            }
        }
        
        protected virtual void ActionByState()
        {
            switch (AIState)
            {
                case AIStateType.Idle:
                    IdleAction();
                    break;
                case AIStateType.Patrolling:
                    PatrollingAction();
                    break;
                case AIStateType.Pursuit:
                    PursuitAction();
                    break;
                case AIStateType.Flee:
                    FleeAction();
                    break;
                case AIStateType.Attack:
                    AttackAction();
                    break;
            }
        }
        protected void IdleAction()
        {
            Target = this.transform.position;
        }
        protected void PatrollingAction()
        {
            var point = PatrollingPoints[CurrentPatrollingPointIndex];
            point.y = this.transform.position.y;
            var distance = (this.transform.position - point).sqrMagnitude;

            if (distance < ArrivalDistance)
            {
                CurrentPatrollingPointIndex = (CurrentPatrollingPointIndex + 1) % PatrollingPoints.Length;
                if (PatrollingDelayCoroutine != null) Debug.LogWarning("Patrolling points are too close. Intersecting arrival distance");
                PatrollingDelayCoroutine = StartCoroutine(PatrollingIdleDelayCor());
            }
            if (PatrollingDelayCoroutine == null) Target = PatrollingPoints[CurrentPatrollingPointIndex];
        }
        protected IEnumerator PatrollingIdleDelayCor()
        {
            yield return new WaitForSeconds(PatrollingIdleDelay + Random.Range(-PatrollingIdleRandomStep, PatrollingIdleRandomStep));
            PatrollingDelayCoroutine = null;
        }
        protected void PursuitAction()
        {
            Target = Player.transform.position;
        }
        protected void FleeAction()
        {
            Target = this.transform.position + Vector3.ClampMagnitude(this.transform.position - Player.transform.position, 5);
        }
        protected void AttackAction()
        {
            (Unit as Enemy).Attack();
        }

        /// <summary>
        /// Main method for AIManager
        /// </summary>
        public void GeneralAICycle()
        {
            if (SpottedPlayer) BattleCycle();
            else SpottedPlayer = PlayerIsInSpotRange;
            ActionByState();
        }
        protected abstract void BattleCycle();


        public event SimpleHandle OnAttack;


        #region Gizmos
        protected void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 0, 0.2f);
            Gizmos.DrawSphere(transform.position, PlayerSpotRange);
            Gizmos.color = new Color(1, 0, 0, 0.2f);
            Gizmos.DrawSphere(transform.position, AttackRange);
        }
        #endregion
    }
}