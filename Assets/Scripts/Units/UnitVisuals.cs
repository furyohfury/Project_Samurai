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
        protected float _blinkTime = 0.1f;


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
            UnitAnimator.SetBool("Moving", true);
            Vector3 animVector = transform.InverseTransformVector(direction);
            UnitAnimator.SetFloat("SMove", animVector.x);
            UnitAnimator.SetFloat("FMove", animVector.z);            
        }
        #endregion


        #region GetDamaged
        public void GetDamagedByProjectile()
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