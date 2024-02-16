using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class HeavyMeleeEnemy : Enemy, IAttackMelee
    {
        protected MeleeWeapon MeleeWeapon;

        protected override void GetDamagedByMelee(MeleeWeapon weapon)
        {
            if ((this.GetType() != weapon.Owner.GetType()) && (this as Enemy == null || weapon.Owner as Enemy == null) && !this.MeleeWeapon.Parrying)
            {
                GetDamaged(weapon.Damage);
            }
        }
    }
}