using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;
using static UnityEngine.InputSystem.InputAction;

namespace Samurai
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class Unit : ColorObject
    {
        [SerializeField]
        protected UnitStatsStruct UnitStats;
        public UnitStatsStruct GetUnitStats()
        {
            return UnitStats;
        }

        protected CharacterController CharController;
        protected UnitInput UnitInput;
        [Inject]
        protected DefaultPlayerGunPool DefPlayerGunPool;

        [SerializeField]
        protected Weapon _unitWeapon;
        public Weapon UnitWeapon { get => _unitWeapon; protected set => _unitWeapon = value; }

        [SerializeField]
        protected Transform WeaponSlot;

        [SerializeField]
        protected float BlinkWhenDamagedTime = 0.1f;


        #region Unity_Methods
        protected virtual void Awake()
        {
            Bindings();
        }
        protected override void Start()
        {
            base.Start();

        }
        protected virtual void Update()
        {
            Movement();

        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Projectile proj))
            {
                GetDamagedByProjectile(proj);
            }
            else if (other.TryGetComponent(out MeleeWeapon weapon))
            {
                GetDamagedByMelee(weapon);
            }
        }
        private void OnDestroy()
        {
            StopAllCoroutines();
        }
        #endregion
        protected void Bindings()
        {
            CharController = GetComponent<CharacterController>();
            UnitInput = GetComponent<UnitInput>();
            UnitWeapon = GetComponentInChildren<Weapon>();
            if (WeaponSlot == null) WeaponSlot = transform.Find("WeaponSlot");
        }


        protected void Movement()
        {
            // Walking
            if (UnitInput.MoveDirection != Vector3.zero && UnitInput.CanMove)
            {
                if (CharController.isGrounded) CharController.Move(UnitStats.MoveSpeed * Time.deltaTime * new Vector3(UnitInput.MoveDirection.x, 0, UnitInput.MoveDirection.z));
                else CharController.Move(Time.deltaTime * (UnitStats.MoveSpeed * new Vector3(UnitInput.MoveDirection.x, 0, UnitInput.MoveDirection.z) + 9.8f * Vector3.down));
            }
        }


        public virtual void UnitShoot()
        {
            if (!UnitWeapon.CanShoot) return;
            UnitWeapon.Shoot();
        }


        public void GetDamagedByProjectile(Projectile proj)
        {
            // if ((proj.Owner as Player != null && this as Enemy != null) || (proj.Owner as Enemy != null && this as Player != null))
            if ((proj.CurrentColor != this.CurrentColor) && (this.GetType() != proj.Owner.GetType()) && (this as Enemy == null || proj.Owner as Enemy == null))
            {
                GetDamaged(proj.GetProjectileStats().Damage);

                var defProj = proj as DefaultPlayerWeaponProjectile;
                if (defProj != null) DefPlayerGunPool.Pool.Release(defProj);
                else Destroy(proj.gameObject);
            }

        }
        protected virtual void GetDamagedByMelee(MeleeWeapon weapon)
        {
            if ((this.GetType() != weapon.Owner.GetType()) && (this as Enemy == null || weapon.Owner as Enemy == null))
            {
                GetDamaged(weapon.Damage);
            }
        }
        protected void GetDamaged(int damage)
        {
            UnitStats.HP -= damage;
            StartCoroutine(GotHitBlink());
            if (UnitStats.HP <= 0)
            {
                UnitInput.UnitInputDie();
            }
        }
        protected IEnumerator GotHitBlink()
        {
            PhaseColor curColor = CurrentColor;
            ChangeColorVisual(PhaseColor.Damaged);
            yield return new WaitForSeconds(0.1f);
            // If when getting damage didnt change color
            if (CurrentColor == curColor) ChangeColorVisual(curColor);
        }
        public virtual void Die()
        {
            Debug.Log($"{gameObject.name} died");
        }

        public virtual void MeleeAttack(CallbackContext _)
        {

        }
    }
}