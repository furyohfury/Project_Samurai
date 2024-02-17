using UnityEngine;
using UnityEngine.AI;
using Zenject;
namespace Samurai
{
    [RequireComponent(typeof(EnemyInput))]
    public abstract class Enemy : Unit
    {
        [Inject]
        protected EnemyPool EnemyPool;
        public NPCAI AI { get; protected set; }



        #region Unity_Methods
        protected override void Awake()
        {
            base.Awake();
            AI = GetComponent<NPCAI>();
        }
        protected override void Start()
        {
            base.Start();
        }
        protected virtual void OnEnable()
        {
            EnemyPool.EnemyList.AddLast(this);
            transform.parent = EnemyPool.transform;
        }
        protected virtual void OnDisable()
        {
            EnemyPool.EnemyList.Remove(this);
        }
        #endregion
        public override void Die()
        {
            Destroy(gameObject);
            //todo What to do with projectiles where he's owner
        }


    }
}