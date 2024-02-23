using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class Arena : MonoBehaviour
    {
        [SerializeField]
        private Transform _floorParent;
        [SerializeField]
        private GameObject[] _floorArray;

        [SerializeField, Space]
        private EnemyPool _enemyPool;


        #region UnityMethods
        private void Awake()
        {
            FloorInitialization();
        }
        private void Start()
        {
            FloorLayerCheck();
        }
        #endregion

        #region Floor
        private void FloorInitialization()
        {
            _floorArray = FindObjectsOfType<GameObject>().Where((go) => go.transform.parent == _floorParent).ToArray();
            if (_floorArray.Length <= 0) Debug.LogError("No floor found in the Floors");
        }
        private void FloorLayerCheck()
        {
            foreach (var go in _floorArray)
            {
                if (go.layer != 1 << 6)
                {
                    go.layer = 1 << 6;
                    Debug.LogWarning($"Floor {gameObject.name} had no floor layer. It's been fixed for this session but needs to be done after that");
                }
            }
        }
        #endregion

        #region EnemyPool
        private void EnemyPoolCheck()
        {

        }
        #endregion
    }
}