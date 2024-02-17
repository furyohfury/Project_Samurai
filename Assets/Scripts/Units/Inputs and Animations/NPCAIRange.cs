using Samurai;
using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class NPCAIRange : NPCAI
    {
        protected RangeWeapon RangeWeapon;

        [SerializeField, Range(0, 100)]
        private int _hpPercentToTryFlee = 30;
        [SerializeField, Range(0, 1f)]
        private float _chanceToFlee = 0.5f;
        [SerializeField, Range(0, 30f)]
        private float _timeToFlee = 5f;
        [SerializeField]
        private float _timeBetweenFleeTries = 2f;

        private bool _fleeing;
        private bool _triedToFlee;

        private Enemy _enemyComponent;

#region UnityMethods
        protected override void Awake()
        {
            base.Awake();
            RangeWeapon = GetComponentInChildren<RangeWeapon>();
            _enemyComponent = GetComponent<Enemy>(); // It would be better if AI didnt know about the enemy component but i dunno how to do that
        }
#endregion
        protected override void BattleCycle()
        {
            // Pursuit if not in attack range
            if (!PlayerIsInAttackRange) 
            {
                AIState = AIStateType.Pursuit;
                return;
            }

            // Trying to flee if low HP
            if (_enemyComponent.GetUnitStats().HP <= _enemyComponent.GetUnitStats().HP * _hpPercentToTryFlee / 100 &&
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

            // Is there an obstacle on the way. Obstacle layer is 6
            if (Physics.Raycast(transform.position, Player.transform.position - transform.position, out RaycastHit hit, AttackRange, 1 << 6)
                && (hit.transform.TryGetComponent(out Obstacle _)
                    || (hit.transform.TryGetComponent(out ColorObstacle colObstacle) && colObstacle.CurrentColor != _enemyComponent.CurrentColor)))
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