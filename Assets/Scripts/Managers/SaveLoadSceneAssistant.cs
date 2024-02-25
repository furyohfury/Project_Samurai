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
            ManageArena();
            LoadPlayerPosition();           
        }
        private void OnDisable()
        {

        }
        #endregion

        private void LoadPlayerPosition()
        {
            _player.UnitPhysics.enabled = false;
            _player.transform.position = SaveLoadManager.CurrentPlayerPosition;
            _player.UnitPhysics.enabled = true;
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