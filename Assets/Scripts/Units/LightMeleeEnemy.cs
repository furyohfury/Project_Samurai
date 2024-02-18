using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class LightMeleeEnemy : Enemy, IAttackMelee
    {
        [SerializeField]
        private MeleeWeapon _meleeWeapon;
        public MeleeWeapon MeleeWeapon { get => _meleeWeapon; private set => _meleeWeapon = value; }

        protected override void Awake()
        {
            base.Awake();
            MeleeWeapon = GetComponentInChildren<MeleeWeapon>();
        }
        public void MeleeAttack()
        {
            throw new System.NotImplementedException();
        }

        protected override void GetDamagedByMelee(MeleeWeapon weapon)
        {
            if ((this.GetType() != weapon.Owner.GetType()) && (this as Enemy == null || weapon.Owner as Enemy == null) && !this.MeleeWeapon.Parrying)
            {
                GetDamaged(weapon.Damage);
            }
        }

    }
}