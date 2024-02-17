using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Samurai
{
    [RequireComponent(typeof(Unit), typeof(Animator))]
    public abstract class UnitInput : MonoBehaviour
    {
        protected Vector3 _movement;
        public ref Vector3 MoveDirection => ref _movement;

        protected Animator UnitAnimator;

        protected Unit Unit;

        public bool CanMove { get; protected set; } = true;

        //TestShit
        public Vector3 TestShit;


        #region Unity_Methods
        protected virtual void Awake()
        {
            Bindings();
        }
        protected virtual void Start()
        {

        }
        protected virtual void Update()
        {
            
        }
        protected virtual void FixedUpdate()
        {
            MovementAnimation();
        }
        protected virtual void Bindings()
        {
            UnitAnimator = GetComponent<Animator>();
            Unit = GetComponent<Unit>();
        }
        protected void MovementAnimation()
        {
            if (MoveDirection != Vector3.zero)
            {
                UnitAnimator.SetBool("Moving", true);
                Vector3 animVector = transform.InverseTransformVector(MoveDirection);
                UnitAnimator.SetFloat("FMove", animVector.z);
                UnitAnimator.SetFloat("SMove", animVector.x);
                // TestShit = animVector;
            }
            else UnitAnimator.SetBool("Moving", false);
        }
        #endregion
        
        public virtual void UnitInputDie()
        {
            UnitAnimator.SetTrigger("Die");            
        }
        public void UnitDieAnimationEnded_UnityEvent() //bind w/ all death animation
        {
            Unit.Die();
        }
    }
}