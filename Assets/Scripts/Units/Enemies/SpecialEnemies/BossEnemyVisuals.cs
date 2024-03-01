using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class BossEnemyVisuals : EnemyVisuals, IMeleeAttack
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
            UnitAnimator.SetTrigger("RangeAttack");
        }
        #endregion
    }
}