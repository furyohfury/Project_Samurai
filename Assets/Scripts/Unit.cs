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

        [SerializeField]
        protected ColorDictionary MaterialColorsDict;

        #region Unity_Methods
        protected virtual void Awake()
        {

        }
        #endregion
    }
}