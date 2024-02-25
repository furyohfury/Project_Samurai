using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class SaveLoadSceneAssistant : MonoBehaviour
    {
        [Inject]
        private Player _player;

        #region UnityMethods
        private void OnEnable()
        {

        }
        private void Start()
        {
            LoadPlayerPosition();
            LoadPlayerRangeWeapon();
            LoadPlayerStats();            
            ManageArena();

        }
        private void OnDisable()
        {

        }
        #endregion

        private void LoadPlayerPosition()
        {
            if (SaveLoadManager.CurrentPlayerPosition == Vector3.zero) return;
            _player.UnitPhysics.enabled = false;
            _player.transform.position = SaveLoadManager.CurrentPlayerPosition;
            _player.UnitPhysics.enabled = true;
        }
        private void LoadPlayerStats()
        {
            _player.SetupPlayer(SaveLoadManager.PlayerStats);
            _player.SetupPlayer(SaveLoadManager.PlayerUnitBuffs);
            _player.SetupPlayer(SaveLoadManager.PlayerOnlyBuffs);

        }
        private void LoadPlayerRangeWeapon()
        {
            _player.SetupPlayer(SaveLoadManager.PlayerRangeWeapon, SaveLoadManager.PlayerPickableWeaponNumberOfBullets);
        }
        private void ManageArena()
        {
            if (SaveLoadManager.CurrentArena == string.Empty) return;


            if (SaveLoadManager.CurrentArenaIsFinished)
            {
                GameObject arena = GameObject.Find(SaveLoadManager.CurrentArena);
                if (arena.TryGetComponent(out Arena arenaComponent))
                {
                    arenaComponent.EntryDoorClose();
                    arenaComponent.FinishedArena();
                }
                else Debug.LogError("Didnt find arena component on arena from savefile");
            }
        }

    }
}