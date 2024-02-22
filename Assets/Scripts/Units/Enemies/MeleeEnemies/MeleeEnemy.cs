using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class MeleeEnemy : Enemy, IMeleeWeapon, IMeleeAttack
    {
        protected override void Bindings()
        {
            base.Bindings();
            MeleeWeaponBindings();
        }

        // For IMeleeWeapon
        #region GetDamaged
        public override void GetDamagedByMelee(MeleeWeapon weapon)
        {
            if (!Parried && !(this is Enemy == weapon.Owner is Enemy))
            {
                UnitVisuals.GetDamagedByMelee();
                ChangeHP(-weapon.Damage);
            }
        }
        #endregion


        // For IMeleeAttack
        #region MeleeAttack
        [SerializeField, Space]
        private MeleeWeapon _meleeWeapon;
        public MeleeWeapon MeleeWeapon { get => _meleeWeapon; private set => _meleeWeapon = value; }

        [SerializeField]
        private float _meleeAttackCooldown = 5f;
        public float MeleeAttackCooldown { get => _meleeAttackCooldown; private set => _meleeAttackCooldown = value; }
        public bool CanHit { get; set; } = true;


        public void MeleeAttack()
        {
            if (CanHit)
            {
                (UnitVisuals as IMeleeAttack).MeleeAttack();
                MeleeWeapon.MeleeAttack();

                StartCoroutine(MeleeAttackCD());
            }
        }
        private IEnumerator MeleeAttackCD()
        {
            CanHit = false;
            yield return new WaitForSeconds(MeleeAttackCooldown);
            CanHit = true;
        }
        public void InMeleeAttack(bool isInMeleeAttack)
        {
            CanMove = !isInMeleeAttack;
        }

        // Parry
        [SerializeField]
        private float _parryInvulTime = 2f;
        public bool Parried { get; set; } = false;

        protected void MeleeWeaponBindings()
        {
            MeleeWeapon.OnParry += Parry;
        }
        public void Parry()
        {
            if (!Parried) StartCoroutine(ParryCoroutine());
        }
        private IEnumerator ParryCoroutine()
        {
            Parried = true;
            (UnitVisuals as PlayerVisuals).Parry();
            yield return new WaitForSeconds(_parryInvulTime);
            Parried = false;
        }
        #endregion

        public override void Attack() => MeleeAttack();
    }
}