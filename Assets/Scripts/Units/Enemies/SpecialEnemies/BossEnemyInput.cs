using System;
using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class BossEnemyInput : EnemyInput
    {

        #region UnityMethods
        private void OnEnable()
        {

        }
        protected override void Start()
        {
            // No patrolling logic so no base.Start()
            Target = Vector3.zero;
            SpottedPlayer = true;
            AIState = AIStateType.Attack;
        }
        private void OnDisable()
        {

        }
        #endregion         

        protected override void AttackAction()
        {
            if (PlayerIsInAttackRange) (Unit as BossEnemy).MeleeAttack();
            else SpecialAttack();
        }

        private void SpecialAttack()
        {
            int r = UnityEngine.Random.Range(0, 3);
            // int r = 2;
            switch (r)
            {
                case 0:
                    if ((Unit as IRangeWeapon).CanShoot && (Unit as BossEnemy).RangeWeapon.CanShoot)
                    {
                        var p = Player.transform.position;
                        p.y = transform.position.y;
                        transform.LookAt(p);
                        RangeAttack();
                    }
                    break;
                case 1:
                    if ((Unit as BossEnemy).CanChargeAttack) ChargeAttack();
                    break;
                case 2:
                    if (Unit.CanMove) JumpToPlayer();
                    break;
                case 3:
                    DistantSlashAttack();                    
                    break;
            }
        }

        private void JumpToPlayer()
        {
            (Unit as BossEnemy).JumpToPlayer();
        }

        private void DistantSlashAttack()
        {
            (Unit as BossEnemy).DistantSlashAttack();
        }

        private void ChargeAttack()
        {
            (Unit as BossEnemy).ChargeAttack();
        }

        private void RangeAttack()
        {
            (Unit as IRangeAttack).RangeAttack();
        }

        protected override void BattleCycle() { }
        public override void Movement() { }
    }
}