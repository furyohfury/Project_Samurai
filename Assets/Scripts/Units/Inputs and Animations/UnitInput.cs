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

        
        // public bool CanHit { get; protected set; } = true;

        protected Unit Unit;

        public bool CanMove { get; protected set; } = true;

        /* public bool Attacking
        {
            get => Attacking;
            protected set
            {
                if (value == true)
                {
                    CanHit = false;
                    CanMove = false;
                }
                else
                {
                    CanHit = true;
                    CanMove = true;
                }
            }
        } */

        // [SerializeField]
        // protected Collider MeleeAttackHitbox;

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
                TestShit = animVector;
            }
            else UnitAnimator.SetBool("Moving", false);
        }
        #endregion
        /* protected virtual void UnitShootAnimation(CallbackContext _)
        {
            if (Unit.UnitWeapon.CanShoot)
            {
                UnitAnimator.SetTrigger("Shoot");
                Unit.UnitShoot();
            }
        } */
        
        public virtual void UnitInputDie()
        {
            UnitAnimator.SetTrigger("Die");
        }
        public void UnitDieAnimationEnded_UnityEvent() //bind w/ all death animation
        {
            Unit.Die();
        }

        /* protected void MeleeAttackAnimation(CallbackContext context)
        {
            if (CanHit) UnitAnimator.SetTrigger("MeleeAttack");
        } */

        #region UnityAnimationEvents
        /* protected virtual void OnShootAnimationStarted_UnityEvent()
        {
            Unit.UnitWeapon.CanShoot = false;
        }
        protected virtual void OnShootAnimationEnded_UnityEvent()
        {
            Unit.UnitWeapon.CanShoot = true;
        } */
        /* protected virtual void OnMeleeAttackAnimationStarted_UnityEvent()
        {
            Attacking = true;
        }
        protected virtual void OnMeleeAttackAnimationEnded_UnityEvent()
        {
            Attacking = false;
        }
        protected virtual void OnMeleeAttackSlashAnimationStarted_UnityEvent()
        {
            MeleeAttackHitbox.enabled = true;
        }
        protected virtual void OnMeleeAttackSlashAnimationEnded_UnityEvent()
        {
            MeleeAttackHitbox.enabled = false;
        } */
        #endregion
    }
}