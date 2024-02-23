using System.Collections;
using UnityEngine;

namespace Samurai
{
    [RequireComponent(typeof(BoxCollider))]
    public class ArenaEnterTrigger : MonoBehaviour
    {
        private BoxCollider _collider;

        private void Awake()
        {
            _collider.isTrigger = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                OnEnterArena?.Invoke();
                _collider.enabled = false;
            }
        }

        public event SimpleHandle OnEnterArena;
    }
}