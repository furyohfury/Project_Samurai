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
        public bool CanHit { get; protected set; } = true;

        protected Unit Unit;

        public bool CanMove { get; protected set; } = true;

        [SerializeField]
        protected Collider MeleeAttackHitbox;

        //TestShit
        public Vector3 TestShit;


        #region Unity_Methods
        protected virtual void Awake()
        {
            UnitAnimator = GetComponent<Animator>();
            Unit = GetComponent<Unit>();
        }
        protected virtual void Start()
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
        protected virtual void UnitShootAnimation(CallbackContext _)
        {
            if (CanShoot)
            {
                UnitAnimator.SetTrigger("Shoot");
                Unit.UnitShoot();
            }
        }
        public virtual void UnitInputDie()
        {
            UnitAnimator.SetTrigger("Die");
        }
        public void UnitDieAnimationEnded_UnityEvent() //bind w/ death animation
        {
            Unit.Die();
        }

        protected void MeleeAttackAnimation(CallbackContext context)
        {
            if (CanHit) UnitAnimator.SetTrigger("MeleeAttack");
        }

        #region UnityAnimationEvents
        protected virtual void OnShootAnimationStarted_UnityEvent()
        {
            CanShoot = false;
        }
        protected virtual void OnShootAnimationEnded_UnityEvent()
        {
            CanShoot = true;
        }
        protected virtual void OnMeleeAttackAnimationStarted_UnityEvent()
        {
            CanMove = false;
            CanShoot = false;
            CanHit = false;
        }
        protected virtual void OnMeleeAttackAnimationEnded_UnityEvent()
        {
            CanMove = true;
            CanShoot = true;
            CanHit = true;
        }
        protected virtual void OnMeleeAttackSlashAnimationStarted_UnityEvent()
        {
            MeleeAttackHitbox.enabled = true;
        }
        protected virtual void OnMeleeAttackSlashAnimationEnded_UnityEvent()
        {
            MeleeAttackHitbox.enabled = false;
        }
        #endregion
    }
}