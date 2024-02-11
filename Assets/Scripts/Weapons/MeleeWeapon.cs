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
        public bool Parrying { get; private set; }

        public Unit Owner;
        private void Start()
        {
            Owner = GetComponentInParent<Unit>();
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<MeleeWeapon>()) Parrying = true;
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<MeleeWeapon>()) Parrying = false;
        }
    }
}