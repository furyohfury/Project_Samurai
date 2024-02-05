using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samurai
{
    public abstract class Unit: ColorObject
    {
        [SerializeField]
        protected UnitStatsStruct UnitStats;
        public UnitStatsStruct GetUnitStats()
        {
            return UnitStats;
        }



        #region Unity_Methods
        protected virtual void Awake()
        {

        }
        #endregion
    }
}