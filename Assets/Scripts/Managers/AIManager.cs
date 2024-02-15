using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using Zenject;
namespace Samurai
{
    public class AIManager : MonoBehaviour
    {
        [Inject]
        private EnemyPool EnemyPool;

        private float FixedDT;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _token;

        private void Start()
        {
            FixedDT = Time.fixedDeltaTime;

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += StopAsyncLogic;
#endif
            foreach (var enemy in EnemyPool.EnemyList)
            {
                enemy.AI.StartingIdlePatrolLogic();
            }

            _cancellationTokenSource = new();
            _token = _cancellationTokenSource.Token;
            StartCoroutine(AILogicManagement());

        }
        private void OnDisable()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= StopAsyncLogic;
#endif
        }
#if UNITY_EDITOR      
        public void StopAsyncLogic(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                _cancellationTokenSource.Cancel();
                Debug.Log("Stopped async AI Logic");
            }
        }
        [ContextMenu("Stop async shit")]
        public void StopAsyncLogic()
        {
            _cancellationTokenSource.Cancel();
            Debug.Log("Stopped async AI Logic");
        }
#endif
        private IEnumerator AILogicManagement()
        {


            while (true)
            {

                foreach (var enemy in EnemyPool.EnemyList.ToList())
                {
                    enemy.AI.GeneralAICycle();
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
