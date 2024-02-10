using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Samurai
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyInput : UnitInput
    {
        protected NavMeshAgent EnemyNavMesh;
        [Inject]
        protected Player Player;
        protected override void Awake()
        {
            base.Awake();
            EnemyNavMesh = GetComponent<NavMeshAgent>();
        }
        protected override void Update()
        {            
            EnemyNavMesh.destination = Player.transform.position;
            MoveDirection = Vector3.ClampMagnitude(EnemyNavMesh.velocity,1);
            base.Update();
        }
    }
}