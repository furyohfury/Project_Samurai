using System.Collections;
using UnityEngine;

namespace Samurai
{
    public interface IAttackRange
    {
        RangeWeapon RangeWeapon { get; }
        void Shoot();
    }
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
        bool CanHit { get;}
        bool InMeleeAttack { get; }
        Collider MeleeAttackHitbox { get;}
        void OnMeleeAttackAnimationStarted_UnityEvent();
        void OnMeleeAttackAnimationEnded_UnityEvent();

        void OnMeleeAttackSlashAnimationStarted_UnityEvent();
        void OnMeleeAttackSlashAnimationEnded_UnityEvent();
    }
}