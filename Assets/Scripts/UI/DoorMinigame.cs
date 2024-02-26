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
        private MMProgressBar _progressBar;
        private BoxCollider _boxCollider;

        
        private void Awake()
        {
            _progressBar = GetComponentInChildren<MMProgressBar>();
            _progressBar.gameObject.SetActive(false);

            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.isTrigger = true;
        }
        private void Update()
        {
            if (_progressBar.BarProgress >= 0.95f)
            {
                _progressBar.gameObject.SetActive(false);
                (_player.UnitInput as PlayerInput).PlayerControls.DoorMinigameMap.Action.performed -= AddValue;
                (_player.UnitInput as PlayerInput).PlayerControls.PlayerMap.Enable();
                
                Destroy(this);
            }

            _progressBar.UpdateBar01(-0.1f * Time.deltaTime);
        }
        private void OnTriggerEnter(Collider other)
        {
            _progressBar.gameObject.SetActive(true);
            (_player.UnitInput as PlayerInput).PlayerControls.PlayerMap.Disable(); 
            (_player.UnitInput as PlayerInput).PlayerControls.DoorMinigameMap.Action.performed += AddValue;
            _boxCollider.enabled = false;
        }


        private void AddValue(CallbackContext _)
        {
            _progressBar.UpdateBar01(0.1f * Time.deltaTime);
        }
    }
}