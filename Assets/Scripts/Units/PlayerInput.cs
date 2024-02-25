using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
namespace Samurai
{
    public class PlayerInput : UnitInput, IRangeAttack, IMeleeAttack
    {
        public PlayerControls PlayerControls { get; private set; }

        public Vector3 MoveDirection { get; private set; }

        #region UnityMethods
        private void OnEnable()
        {
            PlayerControls.Enable();

            PlayerControls.PlayerMap.BlueColor.performed += (cb) => UnitVisuals.ChangeColor(PhaseColor.Blue);
            PlayerControls.PlayerMap.RedColor.performed += (cb) => UnitVisuals.ChangeColor(PhaseColor.Red);
            PlayerControls.PlayerMap.Shoot.performed += RangeAttack;
            PlayerControls.PlayerMap.PickWeapon.performed += EquipPickableRangeWeapon;
            PlayerControls.PlayerMap.MeleeAttack.performed += MeleeAttack;

            PlayerControls.UIMap.Pause.performed += Paused;
        }
        protected void FixedUpdate()
        {
            Movement();
        }
        private void OnDisable()
        {
            PlayerControls.PlayerMap.Shoot.performed -= RangeAttack;
            PlayerControls.PlayerMap.PickWeapon.performed -= EquipPickableRangeWeapon;
            PlayerControls.PlayerMap.MeleeAttack.performed -= MeleeAttack;

            PlayerControls.UIMap.Pause.performed -= Paused;

            PlayerControls.Disable();
            PlayerControls.Dispose();
        }
        #endregion

        #region Bindings
        protected override void Bindings()
        {
            base.Bindings();
            PlayerControls = new();
        }
        public void Paused(CallbackContext cbc)
        {
            (Unit as Player).Paused();
        }
        #endregion


        #region Movement
        public override void Movement()
        {
            Vector2 movement = PlayerControls.PlayerMap.Movement.ReadValue<Vector2>();
            MoveDirection = new Vector3(movement.x, 0, movement.y);
            UnitVisuals.Movement(MoveDirection);
            UnitPhysics.Movement(MoveDirection);
        }
        #endregion

        // IRangeAttack
        #region RangeAttack        
        private void RangeAttack(CallbackContext _) => RangeAttack();
        public void RangeAttack()
        {
            (Unit as IRangeAttack).RangeAttack();
        }
        #endregion

        // Player only
        #region PickableWeapon
        public void EquipPickableRangeWeapon(CallbackContext _)
        {
            (Unit as Player).EquipPickableRangeWeapon((Unit as Player).PickableWeapon);
        }
        #endregion

        //IMeleeAttack
        #region MeleeAttack        
        public void MeleeAttack(CallbackContext _) => MeleeAttack();
        public void MeleeAttack()
        {
            (Unit as IMeleeAttack).MeleeAttack();
        }
        #endregion

    }
}