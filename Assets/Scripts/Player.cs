using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.InputAction;
namespace Samurai
{
    [RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
    public class Player : Unit, IChangeColor
    {
        public Vector3 TestShit;


        private Camera _camera;
        private float _cameraOffset;

        private PlayerInput _input;

        #region Unity_Methods
        protected override void Awake()
        {
            base.Awake();
            _input = GetComponent<PlayerInput>();

            UnitStats = new UnitStatsStruct() { HP = 5, MoveSpeed = 2 };
        }

        private void Start()
        {
            if (_camera == null) _camera = Camera.main;
            _cameraOffset = Vector3.Distance(transform.position, _camera.transform.position);
        }
        private void Update()
        {

            transform.position += Time.deltaTime * UnitStats.MoveSpeed * new Vector3(_input.MoveDirection.x, 0, _input.MoveDirection.y);


            // Facing cursor
            Vector3 cursorPosition = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, _cameraOffset);
            cursorPosition = _camera.ScreenToWorldPoint(cursorPosition);
            TestShit = cursorPosition;
            transform.LookAt(new Vector3(cursorPosition.x, this.transform.position.y, cursorPosition.z));
        }

        #endregion
        public void ChangeColor()
        {
            if (CurrentColor == PhaseColor.Red)
            {
                CurrentColor = PhaseColor.Blue;
            }
            else if (CurrentColor == PhaseColor.Blue)
            {
                CurrentColor = PhaseColor.Red;
            }
            OnPlayerSwapColor?.Invoke();
        }
        public void ChangeColor(PhaseColor color)
        {
            if (color == PhaseColor.Green) CurrentColor = PhaseColor.Green;
        }
        public event SimpleHandle OnPlayerSwapColor;
    }
}