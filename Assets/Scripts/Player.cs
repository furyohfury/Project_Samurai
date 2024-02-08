using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.Windows;
using Zenject;
using static UnityEngine.InputSystem.InputAction;
namespace Samurai
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player : Unit
    {
        public Vector3 TestShit;


        private Camera _camera;
        private float _cameraOffset;

        private UnitInput UnitInput;

        [SerializeField]
        private PlayerWeapon _weapon;
        public PlayerWeapon Weapon
        {
            get => _weapon;
            set => _weapon = value;
        }

        [Inject]
        private DefaultPlayerGunPool _defaultGunPool;
     
        #region Unity_Methods
        protected override void Awake()
        {
            base.Awake();
            UnitInput = GetComponent<PlayerInput>();
        }
        protected override void Start()
        {
            base.Start();
            if (_camera == null) _camera = Camera.main;
        }
        protected override void Update()
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
            base.ChangeColor(color);
            // Notification to obstacles to change color too
            OnPlayerSwapColor?.Invoke(color);
        }

        public override void UnitShoot(CallbackContext _)
        {
            base.UnitShoot();
            if (UnitInput.CanShoot && Weapon == PlayerWeapon.DefaultPistol)
            {
                Projectile proj = _defaultGunPool.Pool.Get();
                proj.SetProjectileOnShoot(this, UnitStats.MoveSpeed, UnitStats.Damage);
                proj.ChangeColor(CurrentColor);
                proj.gameObject.transform.position = transform.position + transform.forward * 0.5f + transform.up * 0.7f;
                proj.transform.rotation = this.transform.rotation;
            }
        }
        protected override void Die()
        {            
            UnitInput.enabled = false;
            Time.timeScale = 0;
            GameObject gameOverScreen = Resources.Load<GameObject>("UI/GameOverScreen");
        }      
        public event ChangeColorHandle OnPlayerSwapColor;
    }
}