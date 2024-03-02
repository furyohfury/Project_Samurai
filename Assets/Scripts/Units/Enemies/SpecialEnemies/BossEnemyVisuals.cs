using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class BossEnemyVisuals : EnemyVisuals, IMeleeAttack, IRangeAttack
    {
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
        public void PrepareChargeAttackAnimation()
        {

        }
        public void ChargeAttackAnimation()
        {

        }
        #endregion
    }
}