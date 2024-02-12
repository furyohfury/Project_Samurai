using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Samurai
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyInput : UnitInput
    {
        protected NavMeshAgent EnemyAgent;
        [Inject]
        protected Player Player;
        protected override void Awake()
        {
            base.Awake();
            EnemyAgent = GetComponent<NavMeshAgent>();
        }
        protected override void Update()
        {            
            EnemyAgent.destination = Player.transform.position;
            MoveDirection = Vector3.ClampMagnitude(EnemyAgent.velocity,1);
            base.Update();
        }
    }
}