using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
namespace Samurai
{
    [RequireComponent(typeof(EnemyInput), typeof(CapsuleCollider))]
    public abstract class Enemy : Unit
    {
        [Inject]
        protected EnemyPool EnemyPool;
        public NPCAI AI { get; protected set; }

        protected MMHealthBar HPBar;



        #region Unity_Methods
        protected override void Awake()
        {
            base.Awake();
            AI = GetComponent<NPCAI>();
            HPBar = GetComponent<MMHealthBar>();
        }
        protected override void Start()
        {
            base.Start();
            HPBar.UpdateBar(UnitStats.HP, 0f, UnitStats.MaxHP, true);
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

        protected override void GetDamaged(int damage)
        {
            HPBar.UpdateBar(UnitStats.HP - damage, 0f, UnitStats.MaxHP, true);
            base.GetDamaged(damage);
        }

        public override void Die()
        {
            Destroy(gameObject);
            //todo What to do with projectiles where he's owner
        }


    }
}