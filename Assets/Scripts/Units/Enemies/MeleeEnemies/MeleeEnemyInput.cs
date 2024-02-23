using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class MeleeEnemyInput : EnemyInput
    {
        [SerializeField]
        private float _fleeingChanceAfterParried = 0.7f;
        [SerializeField]
        private float _fleeTimeAfterParried = 5f;
        private bool _parried;

        #region UnityMethods
        private void OnEnable()
        {
            (Unit as IMeleeWeapon).MeleeWeapon.OnParry += Parried;
        }
        private void OnDisable()
        {
            (Unit as IMeleeWeapon).MeleeWeapon.OnParry -= Parried;
        }
        #endregion

        public override void Movement()
        {
            UnitVisuals.Movement(Target);
            UnitPhysics.Movement(Target);
        }
        protected override void BattleCycle()
        {
            if (_parried) return;
            if (!PlayerIsInAttackRange)
            {
                AIState = AIStateType.Pursuit;
            }
            else
            {
                Target = this.transform.position;
                AIState = AIStateType.Attack;
            }
        }
        protected void Parried()
        {
            if (!_parried && Random.value < _fleeingChanceAfterParried) StartCoroutine(ParriedCoroutine());
        }
        private IEnumerator ParriedCoroutine()
        {
            AIState = AIStateType.Flee;
            _parried = true;
            yield return new WaitForSeconds(_fleeTimeAfterParried);
            _parried = false;
        }        
    }
}