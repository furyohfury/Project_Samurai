using System.Collections;
using UnityEngine;

namespace Samurai
{
    [RequireComponent(typeof(BoxCollider))]
    public class BuffApplier : MonoBehaviour
    {
        [Inject]
        private Collider _boxCollider;

        [SerializeField]
        private UnitBuffsStruct _unitBuffs;
        [SerializeField]
        private PlayerBuffsStruct _playerBuffs;

        private void Awake()
        {
            _boxCollider.isTrigger = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.ApplyBuff(_unitBuffs);

                player.ApplyPlayerBuffs(_playerBuffs);
            }
            Destroy(gameObject);
        }
    }
}