using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            _cancellationTokenSource = new();

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += StopAsyncLogic;
#endif
            foreach (var enemy in EnemyPool.EnemyList)
            {
                enemy.AI.StartingIdlePatrolLogic();
            }

            Debug.Log(Thread.CurrentThread.ManagedThreadId);
            AILogicManagement();

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
            if (state == PlayModeStateChange.ExitingPlayMode) _cancellationTokenSource.Cancel();
        }
        [ContextMenu("Stop async shit")]
        public void StopAsyncLogic()
        {
            _cancellationTokenSource.Cancel();
        }
#endif
        private async void AILogicManagement()
        {
            /* await => Task.Run(() => 
            while (true)
            {
                foreach (var enemy in EnemyPool.EnemyList.ToList())
                {
                    enemy.GeneralAICycle(); // blyat
                }
                Thread.Sleep(Time.fixedDeltaTime * 1000);
            }
            ); //end of task */
            await Task.Run(async () =>
            {
                while (true)
                {
                    foreach (var enemy in EnemyPool.EnemyList.ToList())
                    {
                        enemy.AI.GeneralAICycle();
                    }
                    Task.Delay((int)(FixedDT * 1000));
                    Debug.Log(Thread.CurrentThread.ManagedThreadId);
                }
            }, _token); // end of task
        }
    }
}
