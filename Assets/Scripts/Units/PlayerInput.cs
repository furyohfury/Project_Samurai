using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
namespace Samurai
{
    public class PlayerInput : UnitInput, IRangeAttack, IMeleeAttack
    {
        private PlayerControls _playerControls;

        public Vector3 MoveDirection { get; private set; }

        #region UnityMethods        
        private void OnEnable()
        {
            _playerControls.Enable();

            _playerControls.PlayerMap.BlueColor.performed += (cb) => UnitVisuals.ChangeColor(PhaseColor.Blue);
            _playerControls.PlayerMap.RedColor.performed += (cb) => UnitVisuals.ChangeColor(PhaseColor.Red);

            _playerControls.PlayerMap.Shoot.performed += RangeAttack;
            _playerControls.PlayerMap.PickWeapon.performed += EquipPickableRangeWeapon;
            _playerControls.PlayerMap.PickWeapon.performed += MeleeAttack;
        }
        protected void FixedUpdate()
        {
            Movement();
        }
        private void OnDisable()
        {
            _playerControls.PlayerMap.Shoot.performed -= RangeAttack;
            _playerControls.PlayerMap.PickWeapon.performed -= EquipPickableRangeWeapon;
            _playerControls.PlayerMap.PickWeapon.performed -= MeleeAttack;

            _playerControls.Disable();
        }
        #endregion

        protected override void Bindings()
        {
            _playerControls = new();
        }

        #region Movement
        public override void Movement()
        {
            Vector2 movement = _playerControls.PlayerMap.Movement.ReadValue<Vector2>();
            MoveDirection = new Vector3(movement.x, 0, movement.y);
            UnitVisuals.Movement(MoveDirection);
            UnitPhysics.Movement(MoveDirection);

        }
        #endregion


        #region RangeAttack        
        private void RangeAttack(CallbackContext _) => RangeAttack();
        public void RangeAttack()
        {
            (Unit as IRangeAttack).RangeAttack();
        }
        #endregion


        #region PickableWeapon
        public void EquipPickableRangeWeapon(CallbackContext _)
        {
            (Unit as Player).EquipPickableRangeWeapon();
        }
        #endregion


        #region MeleeAttack        
        public void MeleeAttack(CallbackContext _) => MeleeAttack();
        public void MeleeAttack()
        {
            (Unit as IMeleeAttack).MeleeAttack();
        }
        #endregion

    }
}