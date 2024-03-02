using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class BossEnemyVisuals : EnemyVisuals, IMeleeAttack, IRangeAttack
    {
        #region UnityMethods
        protected override void Start()
        {
            base.Start();
            StartSwappingColors();
        }
        #endregion


        [SerializeField, Space]
        private float _swapColorMaxTime = 9f;
        [SerializeField]
        private MMF_Player _swapColorFeedback;
        #region SwapColor
        private void StartSwappingColors() => StartCoroutine(SwapColorsCor());

        private IEnumerator SwapColorsCor()
        {
            while (true)
            {
                _swapColorFeedback?.PlayFeedbacks();
                if (CurrentColor == PhaseColor.Blue)
                {
                    ChangeColor(PhaseColor.Red);
                }
                else
                {
                    ChangeColor(PhaseColor.Blue);
                }
                yield return new WaitForSeconds(_swapColorMaxTime);
            }            
        }
        #endregion


        // IMeleeAttack
        #region MeleeAttack
        public void MeleeAttack()
        {
            UnitAnimator.SetTrigger("MeleeAttack");
        }


        public void OnMeleeAttackAnimationStarted_UnityEvent()
        {
            (Unit as IMeleeWeapon).InMeleeAttack(true);
        }
        public void OnMeleeAttackAnimationEnded_UnityEvent()
        {
            (Unit as IMeleeWeapon).InMeleeAttack(false);
        }

        public void OnMeleeAttackSlashAnimationStarted_UnityEvent()
        {
            (Unit as IMeleeWeapon).MeleeWeapon.EnableHitbox(true);
        }
        public void OnMeleeAttackSlashAnimationEnded_UnityEvent()
        {
            (Unit as IMeleeWeapon).MeleeWeapon.EnableHitbox(false);
        }
        #endregion

        // IRangeAttack
        #region RangeAttack
        public void RangeAttack()
        {
            StartCoroutine(RecoilMinigun());
        }
        private IEnumerator RecoilMinigun()
        {
            UnitAnimator.SetBool("RangeAttack", true);
            yield return new WaitForSeconds(2f);
            UnitAnimator.SetBool("RangeAttack", false);
        }
        #endregion

        #region ChargeAttack
        [SerializeField]
        public MMF_Player _chargePrepareFeedback;
        [SerializeField]
        public MMF_Player _chargeFeedback;
        [SerializeField, Space]
        public GameObject _warningArrow;
        public void PrepareChargeAttackAnimation(bool began)
        {
            _warningArrow.SetActive(began);
            if (began) _chargePrepareFeedback?.PlayFeedbacks();
        }
        public void ChargeAttackAnimationStarted()
        {
            UnitAnimator.SetBool("ChargeAttack", true);
            _chargeFeedback?.PlayFeedbacks();
        }
        public void ChargeAttackAnimationEnded()
        {
            UnitAnimator.SetBool("ChargeAttack", false);
            if (_chargeFeedback.IsPlaying) _chargeFeedback.StopFeedbacks();
        }

        #endregion

        #region JumpToPlayer
        [SerializeField]
        private MMF_Player _jumpStartFeedback;
        [SerializeField]
        private MMF_Player _jumpEndFeedback;
        public void JumpStart()
        {
            UnitAnimator.SetBool("Jumping", true);
            _jumpStartFeedback?.PlayFeedbacks();
        }
        public void JumpEnd()
        {
            UnitAnimator.SetBool("Jumping", false);
            if (_jumpStartFeedback.isActiveAndEnabled) _jumpStartFeedback?.StopFeedbacks();
            _jumpEndFeedback?.PlayFeedbacks();
        }
        #endregion
        
    }
}