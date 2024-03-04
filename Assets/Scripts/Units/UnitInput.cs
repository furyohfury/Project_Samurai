using System.Collections;
using UnityEditor;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Samurai
{
    public abstract class UnitInput : MonoBehaviour
    {
        [Inject]
        protected Unit Unit;
        [Inject]
        protected UnitVisuals UnitVisuals;
        [Inject]
        protected UnitPhysics UnitPhysics;


        #region UnityMethods
        protected virtual void Awake()
        {
            Bindings();
        }
        #endregion

        protected virtual void Bindings()
        {
            // Unit = GetComponent<Unit>();
            // UnitVisuals = GetComponent<UnitVisuals>();
            // UnitPhysics = GetComponent<UnitPhysics>();
        }


        #region Movement
        /// <summary>
        /// Abstract
        /// </summary>
        public abstract void Movement();
        #endregion


    }
}