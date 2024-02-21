using UnityEngine;
namespace Samurai
{    
    public class PlayerInput : UnitInput, IRangeAttack
    {
        private PlayerControls _playerControls;

        public Vector3 MoveDirection {get; private set;}
        
        #region UnityMethods        
        private void OnEnable()
        {
            _playerControls.Enable();
            
            _playerControls.PlayerMap.BlueColor.performed += (cb) => UnitVisuals.ChangeColor(PhaseColor.Blue);
            _playerControls.PlayerMap.RedColor.performed += (cb) => UnitVisuals.ChangeColor(PhaseColor.Red);

            _playerControls.PlayerMap.Shoot.performed += RangeAttack;

            _playerControls.PlayerMap.PickWeapon.performed += EquipPickableRangeWeapon;
        }
        private void OnDisable()
        {
            _playerControls.PlayerMap.Shoot.performed -= RangeAttack;

            _playerControls.PlayerMap.PickWeapon.performed -= EquipPickableRangeWeapon;

            _playerControls.Disable();
        }
        #endregion

        protected override void Bindings()
        {
            _playerControls = new();
        }
        
        #region Movement
        private override void Movement()
        {
            Vector2 movement = _playerControls.PlayerMap.Movement.ReadValue<Vector2>();
            MoveDirection = new Vector3(movement.x, 0, movement.y);
            if (CanMove)
            {
                UnitVisuals(MoveDirection);
                UnitPhysics(MoveDirection);
            }
            
        }
        #endregion
        

        #region RangeAttack
        public bool CanShoot { get; private set; } = true;
        public void RangeAttack()
        {
            if (CanShoot) (Unit as IRangeAttack).RangeAttack();
        }
        #endregion

        #region PickableWeapon
        public void EquipPickableRangeWeapon(CallbackContext _)
        {
            (Unit as Player).EquipPickableRangeWeapon();
        }
        #endregion

        
        
    }