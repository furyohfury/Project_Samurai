using System.Collections;
using UnityEngine;

namespace Samurai
{
    public abstract class EnemyVisuals : UnitVisuals
    {
        #region UnityMethods
        protected void OnEnable()
        {
            UnitAnimator.enabled = true;
        }
        protected void OnDisable()
        {
            UnitAnimator.enabled = false;
        }
        #endregion

        #region Movement
        public override void Movement(Vector3 direction)
        {
            if (Unit.CanMove && direction != Vector3.zero)
            {
                UnitAnimator.SetBool("Moving", true);
                Vector3 animVector = transform.InverseTransformVector(direction - transform.position);
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
    }
}