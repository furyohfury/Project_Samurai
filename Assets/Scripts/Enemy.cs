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
    }
}