using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        protected void OnTriggerEnter(Collider other)
        {
            GetDamaged(other);
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
        protected virtual void UnitShoot()
        {

        }
        public void GetDamaged(Collider other)
        {
            if (other.TryGetComponent(out Projectile proj))
            {
                // if ((proj.Owner as Player != null && this as Enemy != null) || (proj.Owner as Enemy != null && this as Player != null))
                if ((proj.CurrentColor != this.CurrentColor) && (this.GetType() != proj.Owner.GetType()) && (this as Enemy == null || proj.Owner as Enemy == null))
                {
                    UnitStats.HP -= proj.Damage;
                    if (UnitStats.HP <= 0)
                    {
                        UnitInput.UnitInputDie();                       
                    }
                }
            }
        }
        protected virtual void Die()
        {
            Debug.Log($"{gameObject.name} died");            
        }
    }
}