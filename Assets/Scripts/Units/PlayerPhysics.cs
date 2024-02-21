using UnityEngine;
namespace Samurai
{    
    public class PlayerPhysics : UnitPhysics
    {
        private CharacterController _charController;

        

        protected override void Bindings()
        {
            base.Bindings();
            _charController = GetComponent<CharacterController>();
        }
        

        #region Movement
        protected override void Movement(Vector3 direction)
        {
            if (_charController.isGrounded) _charController.Move(Unit.GetUnitStats().MoveSpeed / Time.timeScale * Time.fixedDeltaTime * new Vector3(direction.x, 0, direction.z));
                else _charController.Move(Time.fixedDeltaTime * (Unit.GetUnitStats().MoveSpeed * new Vector3(direction.x, 0, direction.z) + 9.8f * Vector3.down));
        }
        #endregion
    }