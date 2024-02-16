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


        private void Start()
        {
            foreach (var enemy in EnemyPool.EnemyList)
            {
                enemy.AI.StartingIdlePatrolLogic();
            }

            StartCoroutine(AILogicManagement());

        }
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
