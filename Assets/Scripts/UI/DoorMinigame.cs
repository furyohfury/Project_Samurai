using MoreMountains.Tools;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

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
        private void OnEnable()
        {
            (_player.UnitInput as PlayerInput).PlayerControls.DoorMinigameMap.Action.performed += AddValue;
        }        

        private void Update()
        {
            _progressBar.UpdateBar01(-0.1f * Time.deltaTime);
            if (_progressBar.BarProgress > 0.99f)
            {
                _progressBar.gameObject.SetActive(false);
                (_player.UnitInput as PlayerInput).PlayerControls.PlayerMap.Enable();
                Destroy(this);
            }
        }
        private void OnDisable()
        {
            (_player.UnitInput as PlayerInput).PlayerControls.DoorMinigameMap.Action.performed -= AddValue;
        }
        private void OnTriggerEnter(Collider other)
        {
            _progressBar.gameObject.SetActive(true);
            (_player.UnitInput as PlayerInput).PlayerControls.PlayerMap.Disable(); 
        }


        private void AddValue(UnityEngine.InputSystem.InputAction.CallbackContext _)
        {
            _progressBar.UpdateBar01(0.1f * Time.deltaTime);
        }
    }
}