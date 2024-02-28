using DG.Tweening;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class Arena : MonoBehaviour
    {
        [Inject]
        private Player _player;
        [Inject]
        private SaveLoadSceneAssistant _saveloadSceneAssistant;


        [SerializeField]
        private Transform _floorParent;
        // [SerializeField]
        private GameObject[] _floorArray;

        [SerializeField, Space]
        private EnemyPool _enemyPool;
        [SerializeField]
        private AIManager _aiManager;

        [SerializeField, Space]
        private ArenaEnterTrigger _arenaEnterTrigger;
        [SerializeField]
        private ArenaEnterTrigger _arenaEntryDoorClosingTrigger;
        // [SerializeField]
        // private Obstacle _entryDoor;
        // [SerializeField]
        // private Transform _entryDoorEndLocation;

        [SerializeField, Space]
        private ArenaEndAction[] _arenaEndActions;
        // [SerializeField]
        // private Obstacle _exitDoor;
        // [SerializeField]
        // private Transform _exitDoorEndLocation;
        /* [SerializeField]
        private float _durationToMoveExitDoor = 2; 
        [SerializeField]
        private string _sceneNameToSwitchTo; */


        [SerializeField, Space]
        private MMF_Player _arenaStartedFeedback;
        [SerializeField]
        private MMF_Player _arenaEndedFeedback;
        [SerializeField]
        private MMF_Player _entryDoorCloseFeedback;
        [SerializeField]
        private MMF_Player _exitDoorOpenFeedback;
        [SerializeField]
        private MMF_Player _switchSceneFeedback;


        #region UnityMethods
        private void Awake()
        {
            FloorInit();

            FinishedArenaInit();
        }
        private void OnEnable()
        {
            EnteringInit(true);
            EnemyPoolInit(true);
        }
        private void Start()
        {
            FloorLayerCheck();
            AIManagerCheck();
        }
        private void OnDisable()
        {
            EnteringInit(false);
            EnemyPoolInit(false);
        }
        #endregion

        #region Floor
        private void FloorInit()
        {
            _floorArray = FindObjectsOfType<GameObject>().Where((go) => go.transform.parent == _floorParent).ToArray();
            if (_floorArray.Length <= 0) Debug.LogError("No floor found in the Floors");

            if (_floorArray.Where((floor) => floor.layer != Constants.FloorLayer).ToArray().Length > 0) 
                Debug.LogError($"All floors in {gameObject.name} must have floor layer");

            if (_floorArray.Where((floor) => !floor.isStatic).ToArray().Length > 0)
                Debug.LogError($"All floors in {gameObject.name} must be static");
        }
        private void FloorLayerCheck()
        {
            foreach (var go in _floorArray)
            {
                if (go.layer != Constants.FloorLayer)
                {
                    go.layer = Constants.FloorLayer;
                    Debug.LogWarning($"Floor {gameObject.name} had no floor layer. It's been fixed for this session but needs to be done after that");
                }
            }
        }
        #endregion

        #region EnemyPool
        private void EnemyPoolInit(bool on)
        {
            if (on)
            {
                if (_enemyPool.EnemyList == null || _enemyPool.EnemyList.Count <= 0)
                {
                    Debug.LogError($"Arena {gameObject.name}'s EnemyPool is empty");
                    return;
                }
                _enemyPool.OnAllEnemiesDied += FinishedArena;
            }
            else
            {
                _enemyPool.OnAllEnemiesDied -= FinishedArena;
            }
        }
        private void AIManagerCheck()
        {
            if (_aiManager == null) Debug.LogError($"Arena {gameObject.name} doesnt have a AIManager");
        }
        #endregion

        #region Saving
        private void SaveGame(bool arenaFinished)
        {
            _saveloadSceneAssistant.SaveArena(gameObject.name, arenaFinished);
        }
        #endregion

        #region Entering
        private void EnteringInit(bool on)
        {
            if (on)
            {
                _arenaEnterTrigger.OnEnterArena += PlayerEnteredArena;
                _arenaEntryDoorClosingTrigger.OnEntryDoorClosing += EntryDoorClose;
            }
            else
            {
                _arenaEnterTrigger.OnEnterArena -= PlayerEnteredArena;
                _arenaEntryDoorClosingTrigger.OnEntryDoorClosing -= EntryDoorClose;
                Destroy(_arenaEnterTrigger.gameObject);
            }
        }

        public void EntryDoorClose()
        {
            _entryDoorCloseFeedback?.PlayFeedbacks();
        }

        private void PlayerEnteredArena()
        {
            foreach (var enemy in _enemyPool.EnemyList)
            {
                enemy.UnitVisuals.enabled = true;
                enemy.UnitPhysics.enabled = true;
                enemy.UnitInput.enabled = true;
                enemy.enabled = true;
            }
            _aiManager.enabled = true;
            _arenaEnterTrigger.enabled = false;

            // Saving
            SaveGame(false);

            // Feedbacks
            _arenaStartedFeedback?.PlayFeedbacks();
        }
        #endregion


        #region Finishing
        private void FinishedArenaInit()
        {
            /* if (_arenaEndActions.Contains(ArenaEndAction.OpenDoor) && (_exitDoor == null || _exitDoorEndLocation == null))
            {
                Debug.LogError($"Arena {gameObject.name} must open door but doesnt have one");
            } */

            if (_arenaEndActions.Contains(ArenaEndAction.SwitchScene) && _switchSceneFeedback == null)
            {
                Debug.LogError($"Arena {gameObject.name} must switch scene but doesnt have corresponding feedback");
            }

        }

        private void ClearArena()
        {
            foreach (var enemy in _enemyPool.EnemyList.ToList())
            {
                enemy.DiscardUnit();
            }
        }
        private bool _loaded = false;
        public void FinishedArena()
        {
            if (_enemyPool.EnemyList.Count > 0)
            {
                _loaded = true;
                ClearArena();
                return;
            }
            if (!_loaded) SaveGame(false);
            _aiManager.enabled = false;

            if (_arenaStartedFeedback.IsPlaying) _arenaStartedFeedback?.StopFeedbacks();
            _arenaEndedFeedback?.PlayFeedbacks();

            foreach (var arenaEndAction in _arenaEndActions)
            {
                switch (arenaEndAction)
                {
                    case ArenaEndAction.OpenDoor:
                        /* if (_exitDoor == null || _exitDoorOpenFeedback == null)
                        {
                            Debug.LogError($"Arena {gameObject.name} tried to open exit door but doesn't have it or it's feedback");
                            break;
                        }
                         DOTween.To(() => _exitDoor.transform.position, x => _exitDoor.transform.position = x, _exitDoorEndLocation.transform.position, _durationToMoveExitDoor); */
                        _exitDoorOpenFeedback?.PlayFeedbacks();
                        break;
                    case ArenaEndAction.SwitchScene:
                        _switchSceneFeedback?.PlayFeedbacks();
                        break;
                    case ArenaEndAction.None:
                        break;
                }
            }
        }
        #endregion
    }
}