using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

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
        
        public Weapon UnitWeapon {get; protected set;}

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
            UnitWeapon = GetComponentInChildren<Weapon>();
        }
        protected virtual void Update()
        {
            Movement();

        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Projectile _))
            {
                GetDamaged(other);
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
        }


        protected void Movement()
        {
            // Walking
            if (UnitInput.MoveDirection != Vector3.zero)
            {
                if (CharController.isGrounded) CharController.Move(UnitStats.MoveSpeed * Time.deltaTime * new Vector3(UnitInput.MoveDirection.x, 0, UnitInput.MoveDirection.z));
                else CharController.Move(UnitStats.MoveSpeed * Time.deltaTime * new Vector3(UnitInput.MoveDirection.x, 0, UnitInput.MoveDirection.z) + 9.8f * Time.deltaTime * Vector3.down);
            }
        }


        public virtual void UnitShoot()
        {
            if (!UnitInput.CanShoot) return;
            UnitWeapon.Shoot();
        }


        public void GetDamaged(Collider other)
        {
            if (other.TryGetComponent(out Projectile proj))
            {
                // if ((proj.Owner as Player != null && this as Enemy != null) || (proj.Owner as Enemy != null && this as Player != null))
                if ((proj.CurrentColor != this.CurrentColor) && (this.GetType() != proj.Owner.GetType()) && (this as Enemy == null || proj.Owner as Enemy == null))
                {
                    UnitStats.HP -= proj.GetProjectileStats().Damage;
                    var defProj = proj as DefaultPlayerWeaponProjectile;
                    if (defProj != null) DefPlayerGunPool.Pool.Release(defProj);
                    else Destroy(proj);


                    StartCoroutine(GotHitBlink());
                    if (UnitStats.HP <= 0)
                    {
                        UnitInput.UnitInputDie();                       
                    }
                }
            }
        }
        protected IEnumerator GotHitBlink()
        {
            PhaseColor curColor = CurrentColor;
            ChangeColor(PhaseColor.Damaged);
            yield return new WaitForSeconds(0.1f);
            ChangeColor(curColor);
        }
        public virtual void Die()
        {
            Debug.Log($"{gameObject.name} died");            
        }
    }
}