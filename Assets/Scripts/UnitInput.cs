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
        public bool CanShoot { get; protected set; } = true;

        //TestShit
        public Vector3 TestShit;


        #region Unity_Methods
        protected virtual void Awake()
        {
            UnitAnimator = GetComponent<Animator>();
        }
        private void Start()
        {

        }
        protected virtual void Update()
        {
            if (MoveDirection != Vector3.zero)
            {
                UnitAnimator.SetBool("Moving", true);
                Vector3 animVector = transform.InverseTransformVector(MoveDirection);
                UnitAnimator.SetFloat("FMove", animVector.z);
                UnitAnimator.SetFloat("SMove", animVector.x);
                TestShit = animVector;
            }
            else UnitAnimator.SetBool("Moving", false);

        }
        #endregion
        protected virtual void UnitShoot(CallbackContext _)
        {
            if (CanShoot) UnitAnimator.SetTrigger("Shoot");
        }

    }
}