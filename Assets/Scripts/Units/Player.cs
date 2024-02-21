using UnityEngine;
namespace Samurai
{    
    public class Player : Unit, IRangeAttack
    {      
        private DefaultPlayerWeapon _defaultPlayerWeapon;

        #region UnityMethods
        protected void Start()
        {
            RangeWeapon = _
            EquipRangeWeapon()
        }
        #endregion

        #region RangeAttack
        [SerializeField, Space]
        private RangeWeapon _rangeWeapon;
        public RangeWeapon RangeWeapon { get => _rangeWeapon; private set => _rangeWeapon = value; }

        public void RangeAttack()
        {
            if (RangeWeapon.CanShoot) 
            {
                RangeWeapon.RangeAttack();
                (UnitVisuals as IRangeAttack).RangeAttack();
            }
            
        }
        #endregion
        
        
        #region PickableWeapon
        private RangeWeapon _pickableWeapon;
        public RangeWeapon PickableWeapon
        {
            get => _pickableWeapon; private set => _pickableWeapon = value;
        }
        public void SetPlayerPickableWeapon(RangeWeapon rweapon) => PickableWeapon = rweapon;

        public void EquipRangeWeapon(RangeWeapon rWeapon)
        {
            RangeWeapon = rWeapon;
            RangeWeapon.transform.SetLocalPositionAndRotation(
                RangeWeapon.WeaponPositionWhenPicked, Quaternion.Euler(RangeWeapon.WeaponRotationWhenPicked));

            (UnitVisuals as PlayerVisuals).EquipPickableRangeWeapon(rangeWeapon);
            RangeWeapon.Equipped(this);

            // For UI todo switch to rangeweapon w/out enum
            if (Enum.TryParse(RangeWeapon.GetType().Name, true, out RangeWeaponEnum weapon))
            {
                OnPlayerChangedWeapon?.Invoke(weapon);
            }
            else Debug.LogWarning($"Player equipped weapon not in enum {typeof(RangeWeaponEnum)}");
        }

        public void EquipPickableRangeWeapon(RangeWeapon rweapon)
        {
            if (PickableWeapon == null) return;

            _defaultPlayerWeapon.gameObject.SetActive(false);
            EquipRangeWeapon(rweapon);
            // Throw away empty gun
            RangeWeapon.OnBulletsEnded += UnequipPickableWeapon;
            
            RangeWeapon.transform.parent = WeaponSlot;            
            PickableWeapon = null;
            ((PlayerInput)UnitInput).SetAnimatorController((UnityEditor.Animations.AnimatorController)RangeWeapon.AnimController);
        }

        public void UnequipPickableWeapon()
        {
            Destroy(RangeWeapon.gameObject);
            _defaultPlayerWeapon.gameObject.SetActive(true);
            EquipRangeWeapon(_defaultPlayerWeapon);
        }
        #endregion

        public event SimpleHandle OnPlayerDied();
    }