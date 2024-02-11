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

        public Unit Owner;
        private void Start()
        {
            Owner = GetComponentInParent<Unit>();
        }
        private void OnTriggerEnter(Collider other)
        {
            
        }
    }
}