using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class MeleeUnitInput : EnemyInput, IAttackMelee, IInputMelee
    {
        [SerializeField]
        private MeleeWeapon _meleeWeapon;
        public MeleeWeapon MeleeWeapon { get => _meleeWeapon; private set => _meleeWeapon = value; }

        public bool CanHit { get; private set; } = true;
        // Ne naebatsya s exit time v animatore
        private bool _inMeleeAttack = false;
        public bool InMeleeAttack
        {
            get => _inMeleeAttack;
            private set
            {
                if (value == true)
                {
                    _inMeleeAttack = true;
                    CanMove = false;
                    CanHit = false;
                }
                else
                {
                    _inMeleeAttack = false;
                    CanMove = true;
                    CanHit = true;
                }
            }
        }

        public Collider MeleeAttackHitbox { get; private set; }

        #region UnityMethods
        protected override void Awake()
        {
            base.Awake();
            MeleeWeapon = GetComponentInChildren<MeleeWeapon>();
            MeleeAttackHitbox = MeleeWeapon.GetComponentInChildren<Collider>();
        }
        protected override void Update()
        {
            base.Update();
            if (Vector3.Distance(this.transform.position, EnemyAgent.destination) < 1)
            {
                EnemyAgent.destination = transform.position;
                MeleeAttack();
            }
        }
        #endregion

        public void MeleeAttack()
        {
            if (CanHit)
            {
                UnitAnimator.SetTrigger("MeleeAttack");
                InMeleeAttack = true;
            }            
        }
        #region UnityEvents
        public void OnMeleeAttackAnimationStarted_UnityEvent()
        {
            // InMeleeAttack = true;

        }
        public void OnMeleeAttackAnimationEnded_UnityEvent()
        {
            InMeleeAttack = false;
        }
        public void OnMeleeAttackSlashAnimationStarted_UnityEvent()
        {
            MeleeAttackHitbox.enabled = true;
        }
        public void OnMeleeAttackSlashAnimationEnded_UnityEvent()
        {
            MeleeAttackHitbox.enabled = false;
        }
        #endregion
    }

}