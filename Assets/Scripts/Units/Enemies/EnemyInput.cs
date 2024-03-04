using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Samurai
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class EnemyInput : UnitInput
    {
        [Inject]
        protected Player Player;

        [SerializeField]
        protected AIStateType AIState = AIStateType.Idle;
        // [SerializeField]
        // protected float LogicUpdateTime;

        /// <summary>
        /// Main property for this component to set
        /// </summary>
        public Vector3 Target { get; protected set; }

        // Spotting        
        public bool SpottedPlayer { get; protected set; } = false;
        public void SetSpottedPlayer(bool value) => SpottedPlayer = value;

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
        protected bool CanSeePlayer
        {
            get
            {
                return !Physics.Raycast(transform.position, Player.transform.position, out RaycastHit _,
                    PlayerSpotRange,
                        1 << Constants.ObstacleLayer);
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
        protected Transform[] PatrollingObjectsPoints;
        protected Vector3[] PatrollingPoints;

        protected Vector3 StartPoint;
        protected int CurrentPatrollingPointIndex;
        [SerializeField]
        protected float ArrivalDistance = 0.2f;

        protected Coroutine PatrollingDelayCoroutine;

        #region UnityMethods
        protected virtual void Start()
        {
            List<Vector3> pop = new();
            for (var i = 0; i < PatrollingObjectsPoints.Length; i++)
            {
                pop.Add(PatrollingObjectsPoints[i].transform.position);
            }
            PatrollingPoints = pop.ToArray();
            StartingIdlePatrolLogic();
        }
        protected void FixedUpdate()
        {
            Movement();
        }
        #endregion
        public override void Movement()
        {
            UnitVisuals.Movement(Target);
            UnitPhysics.Movement(Target);
        }

        /// <summary>
        /// First method to run
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
                Target = this.transform.position;
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
        protected virtual void IdleAction()
        {
            Target = Vector3.zero;
        }
        protected virtual void PatrollingAction()
        {
            var point = PatrollingPoints[CurrentPatrollingPointIndex];
            point.y = this.transform.position.y;
            var distance = (this.transform.position - point).sqrMagnitude;

            if (distance < ArrivalDistance * ArrivalDistance)
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
        protected virtual void PursuitAction()
        {
            Target = Player.transform.position;
        }
        protected virtual void FleeAction()
        {
            Target = this.transform.position + Vector3.ClampMagnitude(this.transform.position - Player.transform.position, 5);
        }
        protected virtual void AttackAction()
        {
            transform.LookAt(new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z));
            Target = Vector3.zero;
            (Unit as Enemy).Attack();
        }

        /// <summary>
        /// Main method for AIManager
        /// </summary>
        public void GeneralAICycle()
        {
            if (SpottedPlayer) BattleCycle();
            else SpottedPlayer = (PlayerIsInSpotRange && CanSeePlayer);
            ActionByState();
        }
        /// <summary>
        /// Abstract
        /// </summary>
        protected abstract void BattleCycle();


        // public event SimpleHandle OnAttack;


        #region Gizmos
        protected void OnDrawGizmos()
        {
            // Spot range and attack range
            Gizmos.color = new Color(1, 1, 0, 0.2f);
            Gizmos.DrawSphere(transform.position, PlayerSpotRange);
            Gizmos.color = new Color(1, 0, 0, 0.2f);
            Gizmos.DrawSphere(transform.position, AttackRange);
        }
        #endregion
    }
}