using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class RangeEnemyVisuals : EnemyVisuals, IRangeAttack
    {
        // IRangeAttack
        #region RangeAttack
        public void RangeAttack()
        {
            UnitAnimator.SetTrigger("RangeAttack");
        }
        #endregion
    }
}