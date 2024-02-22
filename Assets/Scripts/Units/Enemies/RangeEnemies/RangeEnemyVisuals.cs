using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class RangeEnemyVisuals : EnemyVisuals
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