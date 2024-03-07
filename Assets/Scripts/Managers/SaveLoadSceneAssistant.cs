using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Samurai
{
    public class SaveLoadSceneAssistant : MonoBehaviour
    {
        [Inject]
        private readonly Player _player;
        [Inject]
        private readonly PlayerUI _playerUI;

        public bool UseSaveFile = true;

        #region UnityMethods
        private void Start()
        {
            SaveLoadManager.SaveLoadManagerInitialization(true);

            if (UseSaveFile)
            {
                if (SaveLoadManager.PlayerDataExist)
                {
                    if (SaveLoadManager.CurrentPlayerPosition != Vector3.zero) LoadAndApplyPlayerPosition();
                    LoadAndApplyPlayerRangeWeapon();
                    LoadAndApplyPlayerStatsAndBuffs();
                }

                ManageArena();
            }

            // WCYD
            _playerUI.Initialize();
        }
        #endregion

        private void LoadAndApplyPlayerPosition()
        {
            // Turning off collider/charcontroller to move player or there'll be and error
            _player.UnitPhysics.enabled = false;
            _player.transform.position = SaveLoadManager.CurrentPlayerPosition;
            _player.UnitPhysics.enabled = true;
        }

        private void LoadAndApplyPlayerRangeWeapon()
        {
            _player.SetupPlayer(SaveLoadManager.PlayerRangeWeapon, SaveLoadManager.NumberOfBulletsForPlayer);
        }

        private void LoadAndApplyPlayerStatsAndBuffs()
        {
            _player.SetupPlayer(SaveLoadManager.PlayerStats);
            _player.SetupPlayer(SaveLoadManager.PlayerUnitBuffs);
            _player.SetupPlayer(SaveLoadManager.PlayerBuffs);
        }

        private void ManageArena()
        {
            if (SaveLoadManager.CurrentArena == string.Empty) return;


            if (SaveLoadManager.CurrentArenaIsFinished)
            {
                GameObject arena = GameObject.Find(SaveLoadManager.CurrentArena);
                if (arena.TryGetComponent(out Arena arenaComponent))
                {
                    arenaComponent.FinishArenaFromSaveFile();
                }
                else Debug.LogError("Didnt find arena component on arena from savefile");
            }
        }

        // Referenced by Arena
        public void SaveArena(string arenaName, bool arenaFinished)
        {
            SaveLoadManager.ArenaSaving(arenaName, arenaFinished, _player);
        }

        // Referenced by PlayerUI
        public void LoadLastCheckpoint()
        {
            SceneManager.LoadScene(SaveLoadManager.CurrentScene);
        }

    }
}