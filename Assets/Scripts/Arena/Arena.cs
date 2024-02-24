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
        [SerializeField]
        private MMF_Player _switchSceneFeedback;

        [SerializeField, Space]
        private MMF_Player _arenaStartedFeedback;
        [SerializeField]
        private MMF_Player _arenaEndedFeedback;
        [SerializeField]
        private MMF_Player _entryDoorCloseFeedback;
        [SerializeField]
        private MMF_Player _exitDoorOpenFeedback;


        #region UnityMethods
        private void Awake()
        {
            FloorInit();
            EnemyPoolInit();
            FinishedArenaInit();
        }
        private void OnEnable()
        {
            EnteringInit(true);
        }
        private void Start()
        {
            FloorLayerCheck();
            AIManagerCheck();
        }
        private void OnDisable()
        {
            EnteringInit(false);
        }
        #endregion

        #region Floor
        private void FloorInit()
        {
            _floorArray = FindObjectsOfType<GameObject>().Where((go) => go.transform.parent == _floorParent).ToArray();
            if (_floorArray.Length <= 0) Debug.LogError("No floor found in the Floors");
        }
        private void FloorLayerCheck()
        {
            foreach (var go in _floorArray)
            {
                if (go.layer != 6)
                {
                    go.layer = 6;
                    Debug.LogWarning($"Floor {gameObject.name} had no floor layer. It's been fixed for this session but needs to be done after that");
                }
            }
        }
        #endregion

        #region EnemyPool
        private void EnemyPoolInit()
        {
            if (_enemyPool.EnemyList == null || _enemyPool.EnemyList.Count <= 0)
            {
                Debug.LogError($"Arena {gameObject.name}'s EnemyPool is empty");
                return;
            }
            _enemyPool.OnAllEnemiesDied += FinishedArena;
        }
        private void AIManagerCheck()
        {
            if (_aiManager == null) Debug.LogError($"Arena {gameObject.name} doesnt have a AIManager");
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

        private void EntryDoorClose()
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

            // Saving
            SaveLoadManager.ArenaSaving(gameObject.name, false, _player) ;

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
        private void FinishedArena()
        {
            SaveLoadManager.ArenaSaving(gameObject.name, true, _player);

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