using System.Collections;
using UnityEngine;

namespace Samurai
{
    [RequireComponent(typeof(MeleeUnitInput))]
    public class NPCAIMelee : NPCAI
    {
        [SerializeField]
        private float _fleeingChanceAfterParried = 0.7;
        [SerializeField]
        private float _fleeTimeAfterParried = 5;
        private Coroutine _fleeAfterParriedCoroutine;

        protected override void BattleCycle()
        {
            if (_fleeAfterParriedCoroutine != null) return;
            if (!PlayerIsInAttackRange)
            {
                AIState = AIStateType.Pursuit;
            }
            else 
            {
                AIState = AIStateType.Attack;
            }
        }
        protected void Parried() // async or coroutine?
        {
            _fleeAfterParriedCoroutine = StartCoroutine(ParriedCoroutine());
        }
        private IEnumerator ParriedCoroutine()
        {
            // Flee if been parried
            if (Random.value < _fleeingChanceAfterParried)
            {
                AIState = AIStateType.Flee;
            }
            yield return new WaitForSeconds(_fleeTimeAfterParried);
            _fleeAfterParriedCoroutine = null;
        }
        
    }
}