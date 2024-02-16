using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Samurai
{
    public class MeleeWeapon : MonoBehaviour
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

        [SerializeField, Tooltip("Time for slow-mo after parry")]
        private float _parrySlowmoTime;
        [SerializeField, Tooltip("Coefficient for timescale")]
        private float _slowMoMultiplier;


        public Unit Owner;
        private void Start()
        {
            Owner = GetComponentInParent<Unit>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MeleeWeapon mw) && 
                (this.GetType() != mw.Owner.GetType()) 
                    && (Owner as Enemy == null || mw.Owner as Enemy == null))
            {
                Parrying = true;
                OnParry?.Invoke();
            }
        }        
        private void OnTriggerExit(Collider other)
        {
            if (Parrying && other.TryGetComponent(out MeleeWeapon mw) 
                    && (this.GetType() != mw.Owner.GetType()) 
                        && (Owner as Enemy == null || mw.Owner as Enemy == null))
            {
                Parrying = false;
            }
        }
        public event SimpleHandle OnParry;
    }
}