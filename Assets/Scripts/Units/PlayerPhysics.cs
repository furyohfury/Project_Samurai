using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
namespace Samurai
{
    public class PlayerPhysics : UnitPhysics
    {
        private CharacterController _charController;

        [Inject]
        private Camera _camera;


        #region UnityMethods
        protected void OnEnable()
        {
            _charController.enabled = true;
        }
        protected void FixedUpdate()
        {
            FaceCursor();
        }
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            EquipPickableRangeWeapon(other);
        }
        protected void OnDisable()
        {
            _charController.enabled = false;
        }        
        #endregion


        protected override void Bindings()
        {
            base.Bindings();
            _charController = GetComponent<CharacterController>();
        }

        private void FaceCursor()
        {
            /* Facing cursor
            var cameraOffset = transform.position.y - _camera.transform.position.y;
            Vector3 cursorPosition = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, cameraOffset);
            cursorPosition = _camera.ScreenToWorldPoint(cursorPosition);
            cursorPosition = new Vector3(cursorPosition.x, this.transform.position.y, cursorPosition.z);
            transform.LookAt(cursorPosition); */

            // WTF
            Ray ray = _camera.ScreenPointToRay((Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 1 << 6))
            {
                float k = (this.transform.y - hit.point.y) / (_camera.transform.position.y - hit.point.y);
                Vector3 cursorPosition = ray.point + (camera.transform.position - hit.point) * k;
                this.transform.LookAt(cursorPosition);
            }

        }

        #region Movement
        public override void Movement(Vector3 direction)
        {
            if (_charController.isGrounded) _charController.Move(Unit.GetUnitStats().MoveSpeed / Time.timeScale * Time.fixedDeltaTime * new Vector3(direction.x, 0, direction.z));
            else _charController.Move(Time.fixedDeltaTime * (Unit.GetUnitStats().MoveSpeed * new Vector3(direction.x, 0, direction.z) + 9.8f * Vector3.down));
        }
        #endregion

        // Player only
        #region PickableWeapon
        public void EquipPickableRangeWeapon(Collider other)
        {
            if (other.TryGetComponent(out RangeWeapon rweapon) && rweapon.IsPickable && rweapon.Owner == null)
            {
                (Unit as Player).SetPlayerPickableWeapon(rweapon);
            }
        }
        #endregion

    }
}