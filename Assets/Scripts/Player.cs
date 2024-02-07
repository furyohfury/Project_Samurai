using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.InputAction;
namespace Samurai
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player : Unit
    {
        public Vector3 TestShit;


        private Camera _camera;
        private float _cameraOffset;

        private PlayerInput _input;

        

        #region Unity_Methods
        protected override void Awake()
        {
            
            _input = GetComponent<PlayerInput>();
            
        }

        private void Start()
        {
            if (_camera == null) _camera = Camera.main;
            
        }
        private void Update()
        {
            base.Update();


            // Facing cursor
            _cameraOffset = Vector3.Distance(transform.position, _camera.transform.position); //todo fix. Must be constant Y
            Vector3 cursorPosition = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, _cameraOffset);
            cursorPosition = _camera.ScreenToWorldPoint(cursorPosition);
            cursorPosition = new Vector3(cursorPosition.x, this.transform.position.y, cursorPosition.z);
            TestShit = cursorPosition;
            transform.LookAt(cursorPosition);
        }

        #endregion
        public override void ChangeColor(PhaseColor color)
        {
            base.ChangeColor();
            // Notification to obstacles to change color too
            OnPlayerSwapColor?.Invoke(color);
        }

        public event ChangeColorHandle OnPlayerSwapColor(PhaseColor color);
    }
}