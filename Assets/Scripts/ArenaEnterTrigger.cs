using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;

namespace Samurai
{
    [RequireComponent(typeof(BoxCollider))]
    public class ArenaEnterTrigger : MonoBehaviour
    {
        [SerializeField]
        private bool isForEntryDoorClosing = false;
        private BoxCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _collider.isTrigger = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                if (isForEntryDoorClosing)
                {
                    OnEntryDoorClosing?.Invoke();
                }
                else
                {
                    OnEnterArena?.Invoke();
                    _collider.enabled = false;
                }                
            }
        }
        public event SimpleHandle OnEntryDoorClosing;
        public event SimpleHandle OnEnterArena;
    }
}