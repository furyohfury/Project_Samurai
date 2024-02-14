using UnityEngine;
using Zenject;
namespace Samurai
{
    public class AIManager : MonoBehaviour
    {
        [Inject]
        private EnemyPool EnemyPool;
        private void Start()
        {
            foreach(var enemy in EnemyPool.EnemyList)
            {
                enemy.StartingIdlePatrolLogic();
            }
            AILogicManagement();
        }
        private async Task AILogicManagement()
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
            while (true)
            {
                foreach (var enemy in EnemyPool.EnemyList.ToList())
                {
                    enemy.GeneralAICycle(); // blyat ne tak
                }
                Task.Delay(Time.fixedDeltaTime * 1000);
            }
        }
    }
}
