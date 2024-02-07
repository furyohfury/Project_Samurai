using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samurai
{
    [RequireComponent(typeof(CharacterController, UnitInput))]
    public abstract class Unit: ColorObject
    {
        [SerializeField]
        protected UnitStatsStruct UnitStats;
        public UnitStatsStruct GetUnitStats()
        {
            return UnitStats;
        }
        
        protected CharacterController CharController;

        protected UnitInput UnitInput;
        #region Unity_Methods
        protected virtual void Awake()
        {
            CharController = GetComponent<CharacterController>();
            UnitInput = GetComponent<UnitUnpit>();
        }
        protected virtual void Update()
        {
            // Walking
            CharController.SimpleMove(UnitStats.MoveSpeed * new Vector3(UnitInput.MoveDirection.x, 0, UnitInput.MoveDirection.z));
        }
        #endregion
    }
}