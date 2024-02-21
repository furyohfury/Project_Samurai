using UnityEngine;
namespace Samurai
{    
    public class PlayerPhysics : UnitPhysics
    {
        private CharacterController _charController;


#region UnityMethods
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter();
            EquipPickableRangeWeapon();
        }
        protected void FixedUpdate()
        {
            FaceCursor();
        }
#endregion     


        protected override void Bindings()
        {
            base.Bindings();
            _charController = GetComponent<CharacterController>();
        }
        
        private void FaceCursor()
        {
            // Facing cursor
            _cameraOffset = Vector3.Distance(transform.position, _camera.transform.position); //todo fix. Must be constant Y
            Vector3 cursorPosition = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, _cameraOffset);
            cursorPosition = _camera.ScreenToWorldPoint(cursorPosition);
            cursorPosition = new Vector3(cursorPosition.x, this.transform.position.y, cursorPosition.z);
            TestShit = cursorPosition;
            transform.LookAt(cursorPosition);
        }

        #region Movement
        protected override void Movement(Vector3 direction)
        {
            if (_charController.isGrounded) _charController.Move(Unit.GetUnitStats().MoveSpeed / Time.timeScale * Time.fixedDeltaTime * new Vector3(direction.x, 0, direction.z));
                else _charController.Move(Time.fixedDeltaTime * (Unit.GetUnitStats().MoveSpeed * new Vector3(direction.x, 0, direction.z) + 9.8f * Vector3.down));
        }
        #endregion
    

        #region PickableWeapon
        public void EquipPickableRangeWeapon()
        {
            if (other.TryGetComponent(out RangeWeapon rweapon) && weapon.IsPickable && weapon.Owner == null)
            {
                (Unit as Player).SetPlayerPickableWeapon(rweapon);
            }
        }
        #endregion


    }