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
        [Inject]
        private PlayerInput _playerInput;
        [SerializeField]
        private Image[] _imagesToChangeColorsAsPlayer;


        #region UnityMethods
        private void OnEnable()
        {
            _player.OnPlayerSwapColor += PlayerChangedColor;
            _player.OnUnitHealthChanged += HealthChanged;
            _player.OnPlayerChangedWeapon += RangeWeaponChanged;
            _player.OnPlayerShot += RangeWeaponNumberOfBulletsChanged;
            _playerInput.OnPlayerAttacked += MeleeWeaponCD;
        }
        private void Start()
        {
            _hpSlider.value = _player.GetUnitStats().HP / _player.GetUnitStats().MaxHP;
            PlayerChangedColor(_player.CurrentColor);
            RangeWeaponChanged(Enum.Parse<RangeWeaponEnum>(_player.RangeWeapon.GetType().Name, true));

        }
        private void OnDisable()
        {
            _player.OnPlayerSwapColor -= PlayerChangedColor;
            _player.OnUnitHealthChanged -= HealthChanged;
            _player.OnPlayerChangedWeapon -= RangeWeaponChanged;
            _player.OnPlayerShot -= RangeWeaponNumberOfBulletsChanged;
            _playerInput.OnPlayerAttacked -= MeleeWeaponCD;
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
            _bulletsNumber.text = _player.RangeWeapon.NumberOfBulletsForPlayer.ToString();
        }
        #endregion


        #region MeleeWeapon
        [SerializeField, Space]
        private Image _meleeWeaponImage;
        [SerializeField]
        private MMFeedbacks _meleeWeaponCDFeedback;

        public void MeleeWeaponCD()
        {
            MMFeedback mf = _meleeWeaponCDFeedback.Feedbacks[0]; //todo fix
            mf.FeedbackDuration = _playerInput.MeleeAttackCooldown;
            mf.Play(_meleeWeaponCDFeedback.transform.position);
        }
        #endregion
    }
}