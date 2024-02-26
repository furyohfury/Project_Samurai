using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Samurai
{
    public class MeleeWeapon : MonoBehaviour, IMeleeAttack
    {
        [SerializeField]
        private int _damage;
        
        public int Damage
        {
            get => _damage;
            protected set => _damage = value;
        }

        [SerializeField]
        private bool _parrying = false;
        public bool Parrying { get => _parrying; set => _parrying = value; }

        public Unit Owner;

        [SerializeField, Space]
        private MMF_Player _meleeAttackStartFeedback;
        [SerializeField]
        private MMF_Player _meleeAttackHitFeedback;
        [SerializeField]
        private MMF_Player _parryFeedback;

        [SerializeField]
        private Collider _hitbox;


        #region UnityMethods
        private void Start()
        {
            Owner = GetComponentInParent<Unit>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MeleeWeapon mw) && !(Owner is Enemy == mw.Owner is Enemy) && !(Owner as IMeleeWeapon).Parried)
            {
                _parryFeedback?.PlayFeedbacks();
                OnParry?.Invoke();
            }
            else if (other.TryGetComponent(out Obstacle _) || other.TryGetComponent(out ColorObstacle _))
            {
                _hitbox.enabled = false;
            }
        }        
        /* private void OnTriggerExit(Collider other)
        {
            if (Parrying && other.TryGetComponent(out MeleeWeapon mw) 
                    && (this.GetType() != mw.Owner.GetType()) 
                        && (Owner as Enemy == null || mw.Owner as Enemy == null))
            {
                Parrying = false;
            }
        } */
        #endregion


        public void MeleeAttack()
        {
            
        }

        public void EnableHitbox(bool isEnabled)
        {
            if (isEnabled) _meleeAttackHitFeedback?.PlayFeedbacks();
            _hitbox.enabled = isEnabled;
        }

        public void ApplyBuff(int damage)
        {
            Damage += damage;
        }

        public event SimpleHandle OnParry;
    }
}