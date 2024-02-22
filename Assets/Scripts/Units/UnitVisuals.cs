using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Samurai
{
    public abstract class UnitVisuals : ColorObject
    {
        protected Unit Unit;

        protected Animator UnitAnimator;

        [SerializeField]
        protected MMF_Player StepsFeedback;

        [SerializeField]
        protected float _blinkTime = 0.1f;

        #region UnityMethods
        protected virtual void Awake()
        {
            Bindings();
        }
        #endregion

        protected virtual void Bindings()
        {
            Unit = GetComponent<Unit>();
            UnitAnimator = GetComponent<Animator>();
        }

        #region Color
        public override void ChangeCurrentColor(PhaseColor color)
        {
            base.ChangeCurrentColor(color);
            Unit.SetCurrentColor(color);
        }
        #endregion

        #region Movement
        public virtual void Movement(Vector3 direction)
        {
            if (Unit.CanMove && direction != Vector3.zero)
            {
                UnitAnimator.SetBool("Moving", true);
                Vector3 animVector = transform.InverseTransformVector(direction);
                UnitAnimator.SetFloat("SMove", animVector.x);
                UnitAnimator.SetFloat("FMove", animVector.z);
                if (!StepsFeedback.IsPlaying) StepsFeedback?.PlayFeedbacks();
            }
            else
            {
                UnitAnimator.SetBool("Moving", false);
                StepsFeedback?.StopFeedbacks();
            }
        }
        #endregion


        #region GetDamaged
        // Different methods in case of need different effects
        public void GetDamagedByProjectile()
        {
            StartCoroutine(GotHitBlink());
        }
        public void GetDamagedByMelee()
        {
            StartCoroutine(GotHitBlink());
        }
        protected IEnumerator GotHitBlink()
        {
            PhaseColor curColor = CurrentColor;
            ChangeColorVisual(PhaseColor.Damaged);
            yield return new WaitForSeconds(_blinkTime);
            // If when getting damage didnt change color
            if (CurrentColor == curColor) ChangeColorVisual(curColor);
        }
        #endregion


        #region Death
        public bool DeathAnimationEnded { get; protected set; } = false;
        public void Die()
        {
            UnitAnimator.SetTrigger("Die");
        }
        public void UnitDieAnimationEnded_UnityEvent() //bind w/ all death animation
        {
            DeathAnimationEnded = true;
        }
        #endregion
    }
}