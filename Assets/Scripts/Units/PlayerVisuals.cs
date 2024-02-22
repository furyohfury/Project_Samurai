using System.Collections;
using UnityEngine;
namespace Samurai
{
    public class PlayerVisuals : UnitVisuals, IRangeAttack, IMeleeAttack
    {
        // Player only
        #region Color
        public override void ChangeColor(PhaseColor color)
        {
            base.ChangeColor(color);
            OnPlayerSwapColor?.Invoke(color);
        }
        #endregion


        // Player only
        #region PickableWeapon

        /* public void EquipRangeWeapon(RangeWeapon rweapon) => SwitchToAnimationLayer(rweapon);
        
        / private string _currentAnimationLayer = typeof(DefaultPlayerWeapon).Name.ToString();

        public void SwitchToAnimationLayer(RangeWeapon rweapon)
        {
            string rwstring = rweapon.GetType().Name;
            if (rwstring != _currentAnimationLayer)
            {
                UnitAnimator.SetLayerWeight(UnitAnimator.GetLayerIndex(_currentAnimationLayer), 0f);
                UnitAnimator.SetLayerWeight(UnitAnimator.GetLayerIndex(rwstring), 1f);
                _currentAnimationLayer = rwstring;
            }
        }*/
        public void EquipRangeWeapon(RangeWeapon rweapon) => SwitchAnimationController(rweapon);
        private void SwitchAnimationController(RangeWeapon rweapon)
        {
            UnitAnimator.runtimeAnimatorController = rweapon.AnimController;
        }

        #endregion


        // IRangeAttack
        #region RangeAttack
        public void RangeAttack()
        {
            UnitAnimator.SetTrigger("RangeAttack");
        }
        #endregion


        // IMeleeAttack
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
            (Unit as IMeleeWeapon).InMeleeAttack(true);

            _sheathedKatana.enabled = false;
            _attackKatana.enabled = true;
            (Unit as IRangeWeapon).RangeWeapon.MeshVisible(false);
        }
        public void OnMeleeAttackAnimationEnded_UnityEvent()
        {
            (Unit as IMeleeWeapon).InMeleeAttack(false);

            _attackKatana.enabled = false;
            _sheathedKatana.enabled = true;
            (Unit as IRangeWeapon).RangeWeapon.MeshVisible(true);
        }

        public void OnMeleeAttackSlashAnimationStarted_UnityEvent()
        {
            (Unit as IMeleeWeapon).MeleeWeapon.EnableHitbox(true);
        }
        public void OnMeleeAttackSlashAnimationEnded_UnityEvent()
        {
            (Unit as IMeleeWeapon).MeleeWeapon.EnableHitbox(true);
        }


        [SerializeField, Space]
        private float _slowMoMultiplier = 0.5f;
        [SerializeField]
        private float _parrySlowmoTime = 3f;
        public void Parry()
        {            
            StartCoroutine(PlayerParrySlomoCor());
        }
        public IEnumerator PlayerParrySlomoCor()
        {
            Time.timeScale = _slowMoMultiplier;
            UnitAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            yield return new WaitForSecondsRealtime(_parrySlowmoTime);
            Time.timeScale = 1;
            UnitAnimator.updateMode = AnimatorUpdateMode.Normal;
        }
        #endregion

        public event ChangeColorHandle OnPlayerSwapColor;
    }
}