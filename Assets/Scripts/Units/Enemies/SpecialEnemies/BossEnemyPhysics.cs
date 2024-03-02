using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class BossEnemyPhysics : EnemyPhysics
    {
        private bool _crashedIntoWall = false;
        public bool CrashedIntoWall { get => _crashedIntoWall; set => _crashedIntoWall = value; }

        /* private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Obstacle _) || collision.gameObject.TryGetComponent(out ColorObstacle _))
            {
                CrashedIntoWall = true;
            }
        } */
        private void FixedUpdate()
        {
            if ((Unit as BossEnemy).CanChargeAttack)
            {
                var v = (Unit as BossEnemy).Player.transform.position; v.y = transform.position.y;
                transform.LookAt(v);
            }
        }


    }
}