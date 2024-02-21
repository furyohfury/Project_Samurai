using System.Collections;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using System;
using TMPro;
using MoreMountains.Feedbacks;

namespace Samurai
{
    public class PlayerUI : MonoBehaviour
    {
        [Inject]
        private Player _player;
        [SerializeField]
        private Image[] _imagesToChangeColorsAsPlayer;


        #region UnityMethods
        private void OnEnable()
        {
            (_player.UnitVisuals as PlayerVisuals).OnPlayerSwapColor += PlayerChangedColor;
            _player.OnUnitHealthChanged += HealthChanged;
            _player.OnPlayerChangedWeapon += RangeWeaponChanged;
            _player.OnPlayerShot += RangeWeaponNumberOfBulletsChanged;
            _player.OnPlayerMeleeHit += MeleeWeaponCD;
        }
        private void Start()
        {
            _hpSlider.value = _player.GetUnitStats().HP / _player.GetUnitStats().MaxHP;
            PlayerChangedColor(_player.CurrentColor);
            RangeWeaponChanged(Enum.Parse<RangeWeaponEnum>(_player.RangeWeapon.GetType().Name, true));
            MeleeWeaponUIInit();
        }
        private void OnDisable()
        {
            (_player.UnitVisuals as PlayerVisuals).OnPlayerSwapColor -= PlayerChangedColor;
            _player.OnUnitHealthChanged -= HealthChanged;
            _player.OnPlayerChangedWeapon -= RangeWeaponChanged;
            _player.OnPlayerShot -= RangeWeaponNumberOfBulletsChanged;
            _player.OnPlayerMeleeHit -= MeleeWeaponCD;
        }

        public void PlayerChangedColor(PhaseColor color)
        {
            var newColor = color switch
            {
                PhaseColor.Blue => Resources.Load<Material>("Materials/BlueColor").color,
                PhaseColor.Red => Resources.Load<Material>("Materials/RedColor").color,
                PhaseColor.Damaged => Resources.Load<Material>("Materials/DamagedColor").color,
                PhaseColor.Green => Resources.Load<Material>("Materials/GreenColor").color,
                PhaseColor.Default => Resources.Load<Material>("Materials/DefaultColor").color,
                _ => Resources.Load<Material>("DefaultColor").color,
            };

            foreach (var image in _imagesToChangeColorsAsPlayer)
            {
                image.color = newColor;
            }
        }
        #endregion

        #region Health
        [SerializeField, Space]
        private Slider _hpSlider;
        public void HealthChanged()
        {
            // w/out float cast just makes it 0
            _hpSlider.value = (float)_player.GetUnitStats().HP / (float)_player.GetUnitStats().MaxHP;
        }
        #endregion


        #region RangeWeapon
        [SerializeField, Space]
        private Image _playerRangeWeaponImage;
        [SerializeField]
        private Image _bulletsImage;
        [SerializeField]
        private WeaponSpriteDictionary _weaponSpriteDict;
        [SerializeField]
        private WeaponSpriteDictionary _bulletsSpriteDict;
        [SerializeField]
        private TextMeshProUGUI _bulletsNumber;

        public void RangeWeaponChanged(RangeWeaponEnum weapon)
        {
            _playerRangeWeaponImage.sprite = _weaponSpriteDict[weapon];
            _bulletsImage.sprite = _bulletsSpriteDict[weapon];
            if (weapon == RangeWeaponEnum.DefaultPlayerWeapon) _bulletsNumber.text = "∞";
            else _bulletsNumber.text = string.Concat("/", _player.RangeWeapon.NumberOfBulletsForPlayer.ToString());
        }

        public void RangeWeaponNumberOfBulletsChanged()
        {
            if (_player.RangeWeapon is DefaultPlayerWeapon) return;
            _bulletsNumber.text = _player.RangeWeapon.NumberOfBulletsForPlayer.ToString();
        }
        #endregion


        #region MeleeWeapon
        [SerializeField, Space]
        private Image _meleeWeaponImage;
        [SerializeField]
        private MMF_Player _meleeWeaponCDFeedback;
        private MMF_ImageFill _meleeWeaponImageFillFeedback;
        private MMF_Pause _meleeWeaponPauseFeedback;

        private void MeleeWeaponUIInit()
        {
            _meleeWeaponImageFillFeedback = _meleeWeaponCDFeedback.GetFeedbackOfType<MMF_ImageFill>();            
            _meleeWeaponPauseFeedback = _meleeWeaponCDFeedback.GetFeedbackOfType<MMF_Pause>();
            _meleeWeaponImageFillFeedback.Duration = _player.MeleeAttackCooldown;
            _meleeWeaponImageFillFeedback.FeedbackDuration = _player.MeleeAttackCooldown;
            _meleeWeaponPauseFeedback.PauseDuration = _player.MeleeAttackCooldown;
            _meleeWeaponImageFillFeedback.ResetFeedback();
        }
        public void MeleeWeaponCD()
        {
            
            // _meleeWeaponPauseFeedback.PauseDuration = _playerInput.MeleeAttackCooldown;
            _meleeWeaponCDFeedback?.PlayFeedbacks();
        }
        #endregion
    }
}