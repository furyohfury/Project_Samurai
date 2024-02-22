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
            StartCoroutine(AILogicManagement());
        }
        private IEnumerator AILogicManagement()
        {
            while (true)
            {
                foreach (var enemy in EnemyPool.EnemyList)
                {
                    if (enemy != null) (enemy.UnitInput as EnemyInput).GeneralAICycle();
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
