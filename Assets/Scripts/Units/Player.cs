using System;
using System.Collections;
using System.Linq;
using UnityEngine;
namespace Samurai
{
    [RequireComponent(typeof(PlayerVisuals))]
    [RequireComponent(typeof(PlayerPhysics))]
    [RequireComponent(typeof(PlayerInput))]
    public class Player : Unit, IRangeAttack, IMeleeAttack, IRangeWeapon, IMeleeWeapon, IHeal
    {
        [SerializeField, Space]
        private PlayerBuffsStruct PlayerBuffs;
        public PlayerBuffsStruct GetPlayerBuffs() => PlayerBuffs;


        [SerializeField, Space]
        private DefaultPlayerWeapon _defaultPlayerWeapon;
        public DefaultPlayerWeapon DefaultPlayerWeapon { get => _defaultPlayerWeapon; private set => _defaultPlayerWeapon = value; }

        [SerializeField]
        private RangeWeapon _rangeWeapon;
        public RangeWeapon RangeWeapon { get => _rangeWeapon; private set => _rangeWeapon = value; }

        [SerializeField]
        private Transform _rangeWeaponSlot;
        public Transform RangeWeaponSlot { get => _rangeWeaponSlot; set => _rangeWeaponSlot = value; }

        public bool CanShoot { get; set; } = true;

        public bool CanTurn {get; private set;} = true;

        #region UnityMethods
        private override void Awake()
        {
            PlayerInitialization();
        }
        #endregion


        // For IMeleeWeapon
        #region GetDamaged
        public override void GetDamagedByMelee(MeleeWeapon weapon)
        {
            if (!Parried && weapon.Owner is Enemy)
            {
                UnitVisuals.GetDamagedByMelee();
                ChangeHP(-weapon.Damage);
            }
        }
        #endregion


        // For IRangeAttack
        #region RangeAttack
        public void RangeAttack()
        {
            if (CanShoot && RangeWeapon.CanShoot)
            {
                RangeWeapon.RangeAttack();
                (UnitVisuals as IRangeAttack).RangeAttack();
                OnPlayerShot?.Invoke();
            }
        }

        public void EquipRangeWeapon(RangeWeapon rWeapon)
        {
            RangeWeapon = rWeapon;
            // RangeWeapon.transform.SetLocalPositionAndRotation(RangeWeapon.WeaponPositionWhenPicked, Quaternion.Euler(RangeWeapon.WeaponRotationWhenPicked));

            (UnitVisuals as PlayerVisuals).EquipRangeWeapon(RangeWeapon);
            RangeWeapon.Equipped(this);

            // For UI todo switch to rangeweapon w/out enum
            if (Enum.TryParse(RangeWeapon.GetType().Name, true, out RangeWeaponEnum weapon))
            {
                OnPlayerChangedWeapon?.Invoke(weapon);
            }
            else Debug.LogWarning($"Player equipped weapon not in enum {typeof(RangeWeaponEnum)}");
        }
        #endregion

        //Player only. PickableWeapon
        #region PickableWeapon
        private RangeWeapon _pickableWeapon;
        public RangeWeapon PickableWeapon
        {
            get => _pickableWeapon; private set => _pickableWeapon = value;
        }
        public void SetPlayerPickableWeapon(RangeWeapon rweapon) => PickableWeapon = rweapon;

        public void EquipPickableRangeWeapon(RangeWeapon pickableWeapon)
        {
            if (pickableWeapon == null) return;

            if (RangeWeapon != _defaultPlayerWeapon) Destroy(RangeWeapon.gameObject);
            _defaultPlayerWeapon.gameObject.SetActive(false);
            pickableWeapon.transform.parent = RangeWeaponSlot;
            EquipRangeWeapon(pickableWeapon);
            // To throw away empty gun
            RangeWeapon.OnBulletsEnded += UnequipPickableWeaponToDefault;
            // Buffs
            RangeWeapon.ApplyBuff(new ProjectileStatsStruct {Damage = PlayerBuffs.PickableWeaponDamageBuff });
            PickableWeapon = null;
        }

        public void UnequipPickableWeaponToDefault()
        {
            Destroy(RangeWeapon.gameObject);
            _defaultPlayerWeapon.gameObject.SetActive(true);
            EquipRangeWeapon(_defaultPlayerWeapon);
        }
        #endregion

        // For IMeleeAttack
        #region MeleeAttack
        [SerializeField, Space]
        private MeleeWeapon _meleeWeapon;
        public MeleeWeapon MeleeWeapon { get => _meleeWeapon; private set => _meleeWeapon = value; }

        [SerializeField]
        private float _meleeAttackCooldown = 5f;
        public float MeleeAttackCooldown { get => _meleeAttackCooldown; set => _meleeAttackCooldown = value; }
        public bool CanHit { get; set; } = true;


        public void MeleeAttack()
        {
            if (CanHit)
            {
                (UnitVisuals as IMeleeAttack).MeleeAttack();

                StartCoroutine(MeleeAttackCD());
                OnPlayerMeleeHit?.Invoke();
            }
        }
        private IEnumerator MeleeAttackCD()
        {
            CanHit = false;
            yield return new WaitForSeconds(MeleeAttackCooldown);
            CanHit = true;
        }
        public void InMeleeAttack(bool isInMeleeAttack)
        {
            // CanMove = !isInMeleeAttack;
            // CanShoot = !isInMeleeAttack;
            // CanTurn = !isInMeleeAttack;
            CanMove = CanShoot = CanTurn = !isInMeleeAttack;
        }

        // Parry
        [SerializeField]
        private float _parryInvulTime = 2f;
        public bool Parried { get; set; } = false;

        private void MeleeWeaponBindings()
        {
            MeleeWeapon.OnParry += Parry;
        }
        private void Parry()
        {
            if (!Parried) StartCoroutine(ParryCoroutine());
        }
        private IEnumerator ParryCoroutine()
        {
            Parried = true;
            (UnitVisuals as PlayerVisuals).ParryPlayerSlomo();
            yield return new WaitForSeconds(_parryInvulTime);
            Parried = false;
        }
        #endregion

        #region Death
        public override void DiscardUnit()
        {
            OnPlayerDied?.Invoke();
        }
        #endregion

        // Player only
        #region PlayerBuffs
        public void ApplyPlayerBuffs(PlayerBuffsStruct playerBuffs)
        {
            // PlayerBuffs.PickableWeaponDamageBuff += playerBuffs.PickableWeaponDamageBuff;
            // PlayerBuffs.SlomoDurationBuff += playerBuffs.SlomoDurationBuff;

            // HUH?
            var playerBuffFields = PlayerBuffs.GetType().GetFields();
            var newBuffFields = playerBuffs.GetType().GetFields();
            object box = PlayerBuffs;

            foreach (var newBuffField in newBuffFields)
            {
                if (newBuffField.FieldType == typeof(int))
                {
                    var corrUnitBuffField = playerBuffFields.Single((f) => f.Name == newBuffField.Name);
                    corrUnitBuffField.SetValue(box, (int)corrUnitBuffField.GetValue(PlayerBuffs) + (int)newBuffField.GetValue(playerBuffs));
                }
                else if (newBuffField.FieldType == typeof(float))
                {
                    var corrUnitBuffField = playerBuffFields.Single((f) => f.Name == newBuffField.Name);
                    corrUnitBuffField.SetValue(box, (float)corrUnitBuffField.GetValue(PlayerBuffs) + (float)newBuffField.GetValue(playerBuffs));
                }
            }
            PlayerBuffs = (PlayerBuffsStruct)box;

            DefaultPlayerWeapon.ApplyBuff(new ProjectileStatsStruct { Damage = PlayerBuffs.DefaultPlayerWeaponDamageBuff });


            /* var playerBuffFields = PlayerBuffs.GetType().GetFields();
            var newBuffFields = playerBuffs.GetType().GetFields();
            foreach (var field in playerBuffFields)
            {
                if (field.GetType() == typeof(int))
                {
                    field.SetValue(this, (int)field.GetValue(this) + (int)(newBuffFields.Single((newbuff) => newbuff.Name == field.Name).GetValue(this)));
                }
                else if (field.GetType() == typeof(float))
                {
                    field.SetValue(this, (float)field.GetValue(this) + (float)(newBuffFields.Single((newbuff) => newbuff.Name == field.Name).GetValue(this)));
                }

            } */
        }
        #endregion

        // Player only
        #region Initialization
        private void PlayerInitialization()
        {
            if (RangeWeapon == null)
            {
                RangeWeapon = GetComponentInChildren<RangeWeapon>();
            }
            EquipRangeWeapon(RangeWeapon);
            MeleeWeaponBindings();
        }
        public void SetupPlayer(UnitStatsStruct unitStats)
        {
            UnitStats = unitStats;
        }
        public void SetupPlayer(UnitBuffsStruct unitBuffs)
        {
            UnitBuffs = unitBuffs;
            ApplyBuff(UnitBuffs);
        }
        public void SetupPlayer(PlayerBuffsStruct playerBuffs)
        {
            PlayerBuffs = playerBuffs;
            ApplyPlayerBuffs(PlayerBuffs);
        }
        public void SetupPlayer(string rWeaponName, int numberOfBullets)
        {
            if (RangeWeapon is Samurai.DefaultPlayerWeapon) return;

            var path = string.Concat("Prefabs/Weapons/", rWeaponName, "_Prefab");
            var rWeapon = Instantiate(Resources.Load<GameObject>(path));
            EquipPickableRangeWeapon(rWeapon.GetComponent<RangeWeapon>());
            RangeWeapon.ApplyBuff(numberOfBullets);
        }
        #endregion


        // Player only
        public void Heal(int hp)
        {
            ChangeHP(hp);
        }

        public void Paused() => OnPlayerPaused?.Invoke();

        public event SimpleHandle OnPlayerDied;
        public event RangeWeaponChangeHandle OnPlayerChangedWeapon;
        public event SimpleHandle OnPlayerShot;
        public event SimpleHandle OnPlayerMeleeHit;
        public event SimpleHandle OnPlayerPaused;
    }
}