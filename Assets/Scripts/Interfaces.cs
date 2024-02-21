using System.Collections;
using UnityEngine;

namespace Samurai
{
    /* public interface IAttackRange
    {
        void RangeAttack();
    } */
    public interface IAttackMelee
    {
        MeleeWeapon MeleeWeapon { get;}
        void MeleeAttack();
    }
    public interface IInputRange
    {
        bool CanShoot { get;}
        void OnShootAnimationStarted_UnityEvent();
        void OnShootAnimationEnded_UnityEvent();
    }
    public interface IInputMelee
    {
        float MeleeAttackCooldown {get;}
        bool CanHit { get;}
        bool InMeleeAttack { get; }
        Collider MeleeAttackHitbox { get;}
        void OnMeleeAttackAnimationStarted_UnityEvent();
        void OnMeleeAttackAnimationEnded_UnityEvent();

        void OnMeleeAttackSlashAnimationStarted_UnityEvent();
        void OnMeleeAttackSlashAnimationEnded_UnityEvent();
    }
    public interface IAttack
    {
        void Attack();
    }

    public interface RangeAttack
    {
        void RangeAttack();
    }
}