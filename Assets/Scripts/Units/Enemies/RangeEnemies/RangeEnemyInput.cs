using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class RangeEnemyInput : EnemyInput
    {
        [SerializeField, Range(0, 100)]
        private int _hpPercentToTryFlee = 30;
        [SerializeField, Range(0, 1f)]
        private float _chanceToFlee = 0.5f;
        [SerializeField, Range(0, 30f)]
        private float _timeToFlee = 5f;
        [SerializeField]
        private float _timeBetweenFleeTries = 2f;

        private bool _fleeing = false;
        private bool _triedToFlee = false;        

        protected override void BattleCycle()
        {
            // Pursuit if not in attack range
            if (!PlayerIsInAttackRange) 
            {
                AIState = AIStateType.Pursuit;
                return;
            }

            // Trying to flee if low HP
            if (Unit.GetUnitStats().HP <= Unit.GetUnitStats().HP * _hpPercentToTryFlee / 100 &&
                !_fleeing && !_triedToFlee)
            {
                if (Random.value < _chanceToFlee)
                {
                    StartCoroutine(FleeCoroutine());
                    return;
                }
                else
                {
                    StartCoroutine(TriedToFlee());
                    return;
                }
            }

            // Is there an obstacle on the way. Obstacle layer is 7
            if (Physics.Raycast(transform.position, Player.transform.position - transform.position, out RaycastHit hit, AttackRange, 1 << 7)
                && (hit.transform.TryGetComponent(out Obstacle _)
                    || (hit.transform.TryGetComponent(out ColorObstacle colObstacle) && colObstacle.CurrentColor != Unit.CurrentColor)))
            {
                AIState = AIStateType.Pursuit;
            }
            else
            {
                Target = this.transform.position;
                AIState = AIStateType.Attack;
            }        
        }
        private IEnumerator FleeCoroutine()
        {
            AIState = AIStateType.Flee;
            _fleeing = true;
            yield return new WaitForSeconds(_timeToFlee);
            _fleeing = false;
        }
        private IEnumerator TriedToFlee()
        {
            _triedToFlee = true;
            yield return new WaitForSeconds(_timeBetweenFleeTries);
            _triedToFlee = false;
        }
    }
}