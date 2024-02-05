using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samurai
{
    [RequireComponent(typeof(Animator))]
    public abstract class Unit: ColorObject
    {
        [SerializeField]
        protected UnitStatsStruct UnitStats;
        public UnitStatsStruct GetUnitStats()
        {
            return UnitStats;
        }

        private Animator _animator;


        #region Unity_Methods
        protected virtual void Awake()
        {
            if (_animator == null) _animator = GetComponent<Animator>();
        }
        #endregion
    }
}