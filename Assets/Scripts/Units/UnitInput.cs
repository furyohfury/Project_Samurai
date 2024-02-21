using System.Collections;
using UnityEditor;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Samurai
{
    [RequireComponent(typeof(Unit))]
    public abstract class UnitInput : MonoBehaviour
    {
        protected Unit Unit;     
        

        #region UnityMethods
        protected virtual void Awake()
        {
            Bindings();
        }
        #endregion

        protected virtual void Bindings()
        {
            Unit = GetComponent<Unit>();
        }

        
        #region Movement
        public abstract void Movement();
        #endregion

        
    }
}