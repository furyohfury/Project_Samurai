using MoreMountains.Feedbacks;
using System.Collections;
using System.Text;
using UnityEngine;
using Zenject;

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

        [SerializeField, Space]
        private MMF_Player _floatingTextFeedback;

        private void Awake()
        {
            _boxCollider.isTrigger = true;
        }
        private void Start()
        {
            var floatingText = _floatingTextFeedback.GetFeedbackOfType<MMF_FloatingText>();
            StringBuilder sb = new();

            var fields = _unitBuffs.GetType().GetFields();
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(float))
                {
                    float fieldValue = (float)field.GetValue(_unitBuffs);
                    if (fieldValue != 0) sb.Append($"+ {fieldValue} to {field.Name}\r\n");
                }
                if (field.FieldType == typeof(int))
                {
                    int fieldValue = (int)field.GetValue(_unitBuffs);
                    if (fieldValue != 0) sb.Append($"+ {fieldValue} to {field.Name}\r\n");
                }
            }
            fields = _playerBuffs.GetType().GetFields();
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(float))
                {
                    float fieldValue = (float)field.GetValue(_playerBuffs);
                    if (fieldValue != 0) sb.Append($"+ {fieldValue} to {field.Name}\r\n");
                }
                if (field.FieldType == typeof(int))
                {
                    int fieldValue = (int)field.GetValue(_playerBuffs);
                    if (fieldValue != 0) sb.Append($"+ {fieldValue} to {field.Name}\r\n");
                }
            }


            floatingText.Value = sb.ToString();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.ApplyBuff(_unitBuffs);

                player.ApplyPlayerBuffs(_playerBuffs);

                _floatingTextFeedback?.PlayFeedbacks();
            }
            _boxCollider.enabled = false;
        }
    }
}