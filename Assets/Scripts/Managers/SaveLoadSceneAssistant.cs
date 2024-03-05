using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class SaveLoadSceneAssistant : MonoBehaviour
    {
        [Inject]
        private readonly Player _player;
        [Inject]
        private readonly PlayerUI _playerUI;

        #region UnityMethods
        private void Start()
        {
#if UNITY_EDITOR
            SaveLoadManager.SaveLoadManagerInitialization(true);
            // SaveLoadManager.LoadLastSave();
#endif 
            var playerData = SaveLoadManager.SaveData.GetField("Player");
            var sceneData = SaveLoadManager.SaveData.GetField("Scene");
            if (!(playerData.isNull || sceneData.isNull || playerData.count <= 0 || sceneData.stringValue.Length <= 0))
            {
                LoadPlayerPosition();
                LoadPlayerRangeWeapon();
                LoadPlayerStats();
                ManageArena();
            }
            _playerUI.Initialize();
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
        public void SaveArena(string arenaName, bool arenaFinished)
        {
            SaveLoadManager.ArenaSaving(arenaName, arenaFinished, _player);
        }

        public void LoadLastCheckpoint()
        {
            SaveLoadManager.LoadLastSave();
        }

    }
}