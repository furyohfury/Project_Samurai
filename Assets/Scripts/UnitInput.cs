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

        //TestShit
        public Vector2 TestShit;
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
                UnitAnimator.SetFloat("FMove", MoveDirection.x);
                UnitAnimator.SetFloat("SMove", MoveDirection.y);
                TestShit = MoveDirection;
            }
            else UnitAnimator.SetBool("Moving", false);

        }
        protected virtual void UnitShoot(CallbackContext context)
        {
            UnitAnimator.SetTrigger("Shoot");
        }

    }
}