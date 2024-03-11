﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Samurai
{
    public class SaveLoadSceneAssistant : MonoBehaviour
    {
        [Inject]
        private readonly SaveLoadManager SaveLoadManager;
        [Inject]
        private readonly RuntimeObjectsCreator _runtimeObjectsCreator;
        [Inject]
        private readonly Player _player;
        [Inject]
        private readonly PlayerUI _playerUI;

        public bool UseSaveFile = true;

        #region UnityMethods
        private void Start()
        {
#if UNITY_EDITOR
            if (UseSaveFile && !SaveLoadManager.Initialized)
            {
                if (SaveLoadManager.SaveData == null || SaveLoadManager.SaveData.count <= 0) SaveLoadManager.SaveLoadManagerInitialization();
                LoadAndApplyPlayerRangeWeapon();
                LoadAndApplyPlayerStatsAndBuffs();
                LoadAndApplyPlayerPosition();
            }
            _playerUI.Initialize();
#else
                UseSaveFile = true;
#endif
        }
        #endregion

        public void InitializeScene(LoadingType type)
        {
            if (UseSaveFile)
            {
                switch (type)
                {
                    case LoadingType.SwitchBetweenLevels:
                        LoadAndApplyPlayerRangeWeapon();
                        LoadAndApplyPlayerStatsAndBuffs();
                        break;
                    case LoadingType.CheckpointReload:
                        LoadAndApplyPlayerPosition();
                        LoadAndApplyPlayerRangeWeapon();
                        LoadAndApplyPlayerStatsAndBuffs();
                        ManageArena();
                        break;
                    case LoadingType.ContinueFromMainMenu:
                        LoadAndApplyPlayerPosition();
                        LoadAndApplyPlayerRangeWeapon();
                        LoadAndApplyPlayerStatsAndBuffs();
                        ManageArena();
                        break;
                    case LoadingType.NewGameFromMainMenu:
                        break;
                    default: break;
                }
            }

            // WCYD
            _playerUI.Initialize();
        }

        private void LoadAndApplyPlayerPosition()
        {
            // Turning off collider/charcontroller to move player or there'll be and error
            _player.UnitPhysics.enabled = false;
            _player.transform.position = SaveLoadManager.CurrentPlayerPosition;
            _player.UnitPhysics.enabled = true;
        }

        private void LoadAndApplyPlayerRangeWeapon()
        {
            if (SaveLoadManager.PlayerRangeWeapon == "DefaultPlayerWeapon") return;

            RangeWeapon rWeapon = _runtimeObjectsCreator.CreateWeapon(SaveLoadManager.PlayerRangeWeapon);
            _player.SetupPlayer(rWeapon, SaveLoadManager.NumberOfBulletsForPlayer);
        }

        private void LoadAndApplyPlayerStatsAndBuffs()
        {
            // _player.SetupPlayer(SaveLoadManager.PlayerUnitStats);
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

        #region Loading_References
        //Referenced by arena
        public void LoadSwitchBetweenLevels(string destinationScene)
        {
            SaveLoadManager.ChangeScene(LoadingType.SwitchBetweenLevels, destinationScene);
        }
        // Referenced by PlayerUI
        public void LoadLastCheckpoint()
        {
            SaveLoadManager.ChangeScene(LoadingType.CheckpointReload);
        }
        public void LoadMainMenu()
        {
            SaveLoadManager.ChangeScene(LoadingType.ToMainMenu);
        }
        #endregion
    }
}