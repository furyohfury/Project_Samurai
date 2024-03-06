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
        private SaveLoadSceneAssistant _saveloadSceneAssistant;


        [SerializeField]
        private Transform _floorParent;
        // [SerializeField]
        // private GameObject[] _floorArray;
        [SerializeField]
        private Transform _obstaclesParent;

        [Inject]
        private EnemyPool _enemyPool;
        [Inject]
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
            ObstaclesInit();
            FinishedArenaInit();
        }
        private void OnEnable()
        {
            EnteringInit(true);
            EnemyPoolInit(true);
        }
        private void Start()
        {
            EntryExitDoorsNonstaticCheck();
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
            var floorArray = FindObjectsOfType<GameObject>().Where((go) => go.transform.parent == _floorParent).ToArray();
            if (floorArray.Length <= 0) Debug.LogError("No floor found in the Floors");

            if (floorArray.Where((floor) => floor.layer != Constants.FloorLayer).ToArray().Length > 0)
                Debug.LogError($"All floors in {gameObject.name} must have floor layer");

            if (floorArray.Where((floor) => !floor.isStatic).ToArray().Length > 0)
                Debug.LogError($"All floors in {gameObject.name} must be static");
        }
        #endregion

        #region Obstacles
        private void ObstaclesInit()
        {
            var obstaclesArray = FindObjectsOfType<GameObject>().Where((obs) => obs.transform.parent == _obstaclesParent).ToArray();
            if (obstaclesArray.Length <= 0) Debug.LogError("No obstacle found in the Obstacles");

            if (obstaclesArray.Where((obs) => obs.layer != Constants.ObstacleLayer).ToArray().Length > 0)
                Debug.LogError($"All obstacles in {gameObject.name} must have obstacle layer");

            if (obstaclesArray.Where((obs) => !obs.isStatic).ToArray().Length > 0)
                Debug.LogError($"All obstacles in {gameObject.name} must be static");
        }
        #endregion

        #region Doors
        private void EntryExitDoorsNonstaticCheck()
        {
            var posFeedback = _entryDoorCloseFeedback.GetFeedbackOfType<MMF_Position>();
            if (posFeedback != null && posFeedback.AnimatePositionTarget.isStatic)
            {
                posFeedback.AnimatePositionTarget.isStatic = false;
                Debug.LogError($"Entry door of arena {gameObject.name} was static");
            }

            posFeedback = _exitDoorOpenFeedback.GetFeedbackOfType<MMF_Position>();
            if (posFeedback != null && posFeedback.AnimatePositionTarget.isStatic)
            {
                posFeedback.AnimatePositionTarget.isStatic = false;
                Debug.LogError($"Exit door of arena {gameObject.name} was static");
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
            _arenaEntryDoorClosingTrigger.gameObject.GetComponent<Collider>().enabled = false;
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
            _arenaEntryDoorClosingTrigger.enabled = false;

            // Saving
            SaveGame(false);

            // Feedbacks
            _arenaStartedFeedback?.PlayFeedbacks();
        }
        #endregion


        #region Finishing
        private void FinishedArenaInit()
        {
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
    

        // Referenced by SaveLoadSceneAssistant
        public void FinishArenaFromSaveFile()
        {
            EntryDoorClose();
            FinishedArena();
        }
    }
}