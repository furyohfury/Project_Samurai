using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Samurai
{
    [RequireComponent(typeof(EnemyInput), typeof(Enemy))]
    public abstract class NPCAI : MonoBehaviour
    {
        [Inject]
        protected Player Player;

        public Vector3 Target {get; protected set;}
        
        // Spotting
        [SerializeField]
        protected float _playerSpotRange;
        protected bool PlayerIsInSpotRange
        {
            get 
            {
                Vector3 distanceToPlayer = Player.transform.position - this.transform.position;
                return distanceToPlayer.sqrMagnitude < _playerSpotRange * _playerSpotRange;
            }
        }

        //Attacking
        [SerializeField]
        protected float _attackRange;
        protected bool PlayerIsInAttackRange
        {
            get 
            {
                Vector3 distanceToPlayer = Player.transform.position - this.transform.position;
                return distanceToPlayer.sqrMagnitude < _attackRange * _attackRange;
            }
        }        
        
        //Patrolling
        protected Vector3 StartPoint;
        protected int CurrentPatrollingPointIndex;
        [SerializeField]
        protected float PatrollingIdleDelay;
        [SerializeField]
        protected float PatrollingIdleRandomStep;
        protected Coroutine PatrollingDelayCoroutine;
        protected Vector3[] PatrollingPoints;
        protected float ArrivalDistance;

        
        protected AIStateType AIState;
        [SerializeField]
        protected float _logicUpdateTime;
#region UnityMethods
        protected virtual void Start()
        {
            StartingIdlePatrolLogic();
        }

#endregion
        protected void StartingIdlePatrolLogic()
        {
            if (PatrollingPoints != null && PatrollingPoints.Length > 1)
            {
                // Level patrolling points with NPC
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

        protected abstract void UpdateState(AIStateType stateType);
        protected void IdleCycle()
        {

        }
        protected void PatrollingCycle()
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
        protected void PursuitCycle()
        {
            Target = Player.transform.position;
        }
        protected void FleeCycle()
        {
            Target = this.transform.position + Vector3.ClampMagnitude(this.transform.position - Player.transform.position, 5);
        }
        protected void AttackCycle()
        {
            OnAttack?.Invoke();
        }
        public event SimpleHandle OnAttack;
    }
}