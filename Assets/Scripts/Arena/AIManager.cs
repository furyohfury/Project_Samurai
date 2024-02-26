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
        [SerializeField]
        private EnemyPool EnemyPool;

        private bool _aggroed = false;

        private void Awake()
        {
            if (EnemyPool == null) Debug.LogError($"AIManager {gameObject.name} doesnt have an EnemyPool");
        }
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
                    if (enemy != null && enemy.UnitInput.enabled) (enemy.UnitInput as EnemyInput).GeneralAICycle();
                    if (!_aggroed && (enemy.UnitInput as EnemyInput).SpottedPlayer)) AggroAllEnemies();
                }
                yield return new WaitForFixedUpdate();
            }
        }
        
        private void AggroAllEnemies()
        {
            foreach(var enemy in EnemyPool.EnemyList)
            {
                if (enemy != null && enemy.UnitInput.enabled) (enemy.UnitInput as EnemyInput).SetSpottedPlayer(true);
            }
            _aggroed = true;
        }
    }
}
