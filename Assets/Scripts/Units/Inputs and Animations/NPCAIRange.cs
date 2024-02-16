using Samurai;
using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class NPCAIRange : NPCAI
    {
        protected RangeWeapon RangeWeapon;

        [SerializeField]
        private int _hpPercentToTryFlee = 30;
        [SerializeField, Range(0, 1)]
        private float _chanceToFlee = 0.5f;

#region UnityMethods
        protected override void Awake()
        {
            base.Awake();
            RangeWeapon = GetComponentInChildren<RangeWeapon>();
        }
#endregion
        protected override void BattleCycle()
        {
            if (!PlayerIsInAttackRange) AIState = AIStateType.Pursuit;
            else
            {

            }
        }        
    }
}