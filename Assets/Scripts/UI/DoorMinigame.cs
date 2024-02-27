using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System;
using System.Collections;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace Samurai
{
    public class DoorMinigame : MonoBehaviour
    {
        [Inject]
        private Player _player;

        // Bump event is connected to sound and camera aoom
        private MMProgressBar _progressBar;
        [SerializeField, Tooltip("Doesnt work now")]
        private float _decreasingSpeed = 0.0001f;
        [SerializeField]
        private float _maxAddValue = 0.15f;

        private BoxCollider _boxCollider;
        [SerializeField]
        private Canvas _canvas;

        [SerializeField, Space]
        private MMF_Player _arrowDownFeedback;
        [SerializeField]
        private MMF_Player _addValueFeedback;


        private void Awake()
        {
            _progressBar = GetComponentInChildren<MMProgressBar>();
            

            _boxCollider = GetComponent<BoxCollider>();
            
        }
        private void Start()
        {
            _canvas.gameObject.SetActive(false);
            _boxCollider.isTrigger = true;
        }
        private void Update()
        {
            if (_progressBar.BarProgress >= 0.95f)
            {
                _canvas.gameObject.SetActive(false);
                (_player.UnitInput as PlayerInput).PlayerControls.DoorMinigameMap.Action.performed -= AddValue;
                (_player.UnitInput as PlayerInput).PlayerControls.PlayerMap.Enable();
                OnFinishedDoorMinigame?.Invoke();
                Destroy(this);
            }
            // _progressBar.UpdateBar01(_progressBar.BarProgress - _decreasingSpeed);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player _))
            {
                _canvas.gameObject.SetActive(true);
                (_player.UnitInput as PlayerInput).PlayerControls.PlayerMap.Disable();
                (_player.UnitInput as PlayerInput).PlayerControls.DoorMinigameMap.Action.performed += AddValue;
                _boxCollider.enabled = false;
                _arrowDownFeedback?.PlayFeedbacks();
            }
            
        }


        private void AddValue(CallbackContext _)
        {
            _progressBar.UpdateBar01(_progressBar.BarProgress + UnityEngine.Random.Range(0, _maxAddValue));
            _addValueFeedback?.PlayFeedbacks();
        }

        public event SimpleHandle OnFinishedDoorMinigame;
    }
}