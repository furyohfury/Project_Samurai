using MoreMountains.Feedbacks;
using System;
using System.Collections;
using UnityEngine;
using Zenject;
namespace Samurai
{
    public class BossEnemy : Enemy, IMeleeAttack, IMeleeWeapon, IRangeAttack, IRangeWeapon
    {
        [Inject]
        private readonly Player _player;

        protected override void Bindings()
        {
            base.Bindings();
            MeleeWeaponBindings();
            if (RangeWeapon == null) RangeWeapon = GetComponentInChildren<RangeWeapon>();
            EquipRangeWeapon(RangeWeapon);
        }

        // For IMeleeWeapon
        #region GetDamaged
        public override void GetDamagedByMelee(MeleeWeapon weapon)
        {
            if (!Parried && weapon.Owner is not Enemy)
            {
                UnitVisuals.GetDamagedByMelee();
                ChangeHP(-weapon.Damage);
            }
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
                MeleeWeapon.MeleeAttack();

                StartCoroutine(MeleeAttackCD());
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
            CanMove = !isInMeleeAttack;
            CanShoot = !isInMeleeAttack;
        }

        // Parry
        [SerializeField]
        private float _parryInvulTime = 2f;
        public bool Parried { get; set; } = false;

        protected void MeleeWeaponBindings()
        {
            MeleeWeapon.OnParry += Parry;
        }
        public void Parry()
        {
            if (!Parried) StartCoroutine(ParryCoroutine());
        }
        private IEnumerator ParryCoroutine()
        {
            Parried = true;
            yield return new WaitForSeconds(_parryInvulTime);
            Parried = false;
        }
        #endregion


        // IRangeWeapon
        #region RangeWeapon
        [SerializeField]
        private RangeWeapon _rangeWeapon;
        public RangeWeapon RangeWeapon { get => _rangeWeapon; private set => _rangeWeapon = value; }

        [SerializeField]
        private Transform _rangeWeaponSlot;
        public Transform RangeWeaponSlot { get => _rangeWeaponSlot; set => _rangeWeaponSlot = value; }

        public bool CanShoot { get; set; } = true;
        #endregion


        // For IRangeAttack
        #region RangeAttack
        public void RangeAttack()
        {
            if (CanShoot && RangeWeapon.CanShoot)
            {
                RangeWeapon.RangeAttack();
                (UnitVisuals as IRangeAttack).RangeAttack();
            }
        }

        public void EquipRangeWeapon(RangeWeapon rWeapon)
        {
            RangeWeapon = rWeapon;
            // RangeWeapon.transform.SetLocalPositionAndRotation(RangeWeapon.WeaponPositionWhenPicked, Quaternion.Euler(RangeWeapon.WeaponRotationWhenPicked));

            RangeWeapon.Equipped(this);
        }
        #endregion
        public override void Attack()
        {
            throw new System.NotImplementedException();
        }

        #region ChargeAttack
        public bool CanChargeAttack = true;
        [SerializeField, Space, Range(0, 5f)]
        private float _chargePrepareTime = 2f;
        [SerializeField, Range(0, 10f)]
        private float _chargeSpeedMultiplier = 5;

        [SerializeField, Space]
        private GameObject _warningArrow;
        [SerializeField]
        private MeleeWeapon _chargeMeleeWeapon;
        [SerializeField]
        private MMF_Player _startingChargeFeedback;
        [SerializeField]
        private MMF_Player _chargeFeedback;
        public void ChargeAttack()
        {
            CanMove = false;
            CanShoot = false;
            CanHit = false;
            CanChargeAttack = false;
            StartCoroutine(ChargeAttackCoroutine());
        }
        private IEnumerator ChargeAttackCoroutine()
        {
            // Prepare            
            _warningArrow.SetActive(true);
            _startingChargeFeedback?.PlayFeedbacks();

            (UnitVisuals as BossEnemyVisuals).PrepareChargeAttackAnimation();

            float count = 0f;
            while (count < _chargePrepareTime)
            {
                // Look at player
                Vector3 v = new(_player.transform.position.x, this.transform.position.y, _player.transform.position.z);
                transform.LookAt(v);
                count += Time.deltaTime;
                yield return null;
            }
            _warningArrow.SetActive(false);

            // Charge
            _chargeMeleeWeapon.gameObject.SetActive(true);
            _chargeMeleeWeapon.EnableHitbox(true);
            (UnitVisuals as BossEnemyVisuals).ChargeAttackAnimation();
            _chargeFeedback?.PlayFeedbacks();
            Vector3 forward = transform.forward;
            while (!(UnitPhysics as BossEnemyPhysics).CrashedIntoWall)
            {
                Physics.Raycast(transform.position, forward, out RaycastHit hit, float.MaxValue, 1 << Constants.ObstacleLayer);
                transform.position += Time.deltaTime * UnitStats.MoveSpeed * _chargeSpeedMultiplier * forward;
                yield return null;
            }
            (UnitPhysics as BossEnemyPhysics).CrashedIntoWall = false;

            _chargeMeleeWeapon.EnableHitbox(false);

            CanMove = true;
            CanShoot = true;
            CanHit = true;
            CanChargeAttack = true;
        }
        #endregion

        #region DistantSlashAttack
        public void DistantSlashAttack()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}