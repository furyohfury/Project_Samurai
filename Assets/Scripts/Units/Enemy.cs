using UnityEngine;
namespace Samurai
{
    [RequireComponent(typeof(EnemyInput))]
    public abstract class Enemy : Unit
    {
        protected override void Start()
        {
            base.Start();
        }
        public void ChangeColor()
        {
            
        }
        public override void Die()
        {
            base.Die();
            Destroy(gameObject);
            //todo remove from pools
            //todo What to do with projectiles where he's owner
        }
    }
}