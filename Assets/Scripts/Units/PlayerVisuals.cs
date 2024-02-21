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
        

        #region MeleeAttack
        public void MeleeAttack()
        {
            UnitAnimator.SetTrigger("MeleeAttack");
        }

        [SerializeField, Space]
        private MeshRenderer _sheathedKatana;
        [SerializeField]
        private MeshRenderer _attackKatana;

        public void OnMeleeAttackAnimationStarted_UnityEvent()
        {   
            (Unit as IMeleeAttack).InMeleeAttack(true);         
            
            _sheathedKatana.enabled = false;
            _attackKatana.enabled = true;
            _player.RangeWeapon.gameObject.SetActive(false);
        }
        public void OnMeleeAttackAnimationEnded_UnityEvent()
        {
            (Unit as IMeleeAttack).InMeleeAttack(false);

            _attackKatana.enabled = false;
            _sheathedKatana.enabled = true;
            _player.RangeWeapon.gameObject.SetActive(true);
        }


        [SerializeField]
        private float _slowMoMultiplier = 0.5f;
        [SerializeField]
        private float _parrySlowmoTime = 3f;
        public void Parry()
        {
            Time.timeScale = _slowMoMultiplier;
            UnitAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            yield return new WaitForSecondsRealtime(_parrySlowmoTime);
            Time.timeScale = 1;
            UnitAnimator.updateMode = AnimatorUpdateMode.Normal;
        }
        #endregion
    }