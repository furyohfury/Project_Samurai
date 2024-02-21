using UnityEngine;
namespace Samurai
{    
    public class PlayerVisuals : UnitVisuals, IRangeAttack
    {
        #region PickableWeapon
        // private RangeWeaponEnum _currentAnimationLayer = RangeWeaponEnum.DefaultPlayerWeapon;

        // Layers must have names of rangeweaponenum
        /* public void SwitchToAnimationLayer(RangeWeaponEnum rweapon)
        {
            if (rweapon != _currentAnimationLayer)
            {
                UnitAnimator.SetLayerWeight(UnitAnimator.GetLayerIndex(_currentAnimationLayer.ToString()), 0f);
                UnitAnimator.SetLayerWeight(UnitAnimator.GetLayerIndex(rweapon.ToString()), 1f);
                _currentAnimationLayer = rweapon;
            }            
        } */

        public void EquipRangeWeapon(RangeWeapon rweapon) => SwitchToAnimationLayer(rweapon);

        private string _currentAnimationLayer = DefaultPlayerWeapon.GetType().Name;

        public void SwitchToAnimationLayer(RangeWeapon rweapon)
        {
            string rwstring = rweapon.GetType().Name;
            if (rwstring != _currentAnimationLayer)
            {
                UnitAnimator.SetLayerWeight(UnitAnimator.GetLayerIndex(_currentAnimationLayer), 0f);
                UnitAnimator.SetLayerWeight(UnitAnimator.GetLayerIndex(rwstring), 1f);
                _currentAnimationLayer = rwstring;
            }            
        }
        #endregion


        #region RangeAttack
        public void RangeAttack()
        {
            UnitAnimator.SetTrigger("RangeAttack");
        }
        #endregion
        
    }