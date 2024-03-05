using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System;
using System.Collections;
using UnityEngine;
using Zenject;
namespace Samurai
{
    public class BossEnemy : Enemy, IMeleeAttack, IMeleeWeapon, IRangeAttack, IRangeWeapon
    {
        [Inject]
        public readonly Player Player;

        #region UnityMethods
        protected override void OnEnable()
        {
            base.OnEnable();
            HPBar.transform.parent.gameObject.SetActive(true);
        }
        #endregion

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
                CanHit = false;
                (UnitVisuals as IMeleeAttack).MeleeAttack();
                StartCoroutine(MeleeAttackCD());
            }
        }
        private IEnumerator MeleeAttackCD()
        {
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
        [SerializeField, Range(0, 20f)]
        private float _chargeSpeedMultiplier = 5;
        [SerializeField]
        private int _maxNumberOfChargeAttacks = 3;
        [SerializeField]
        private float _timeBetweenCharges = 1f;
        [SerializeField]
        private MeleeWeapon _chargeMeleeWeapon;

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
            // _chargeMeleeWeapon.gameObject.SetActive(true);
            for (var i = 0; i < UnityEngine.Random.Range(1, _maxNumberOfChargeAttacks); i++)
            {
                // Prepare                
                (UnitVisuals as BossEnemyVisuals).PrepareChargeAttackAnimation(true);

                float count = 0f;
                while (count < _chargePrepareTime)
                {
                    // Look at player
                    Vector3 v = new(Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
                    transform.LookAt(v);
                    count += Time.deltaTime;
                    yield return null;
                }
                (UnitVisuals as BossEnemyVisuals).PrepareChargeAttackAnimation(false);

                // Charge
                _chargeMeleeWeapon.EnableHitbox(true);
                (UnitVisuals as BossEnemyVisuals).ChargeAttackAnimationStarted();

                // Getting end position in wall
                Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, float.MaxValue, 1 << Constants.ObstacleLayer);
                Vector3 endpos = hit.point;
                endpos.y = transform.position.y;

                // Moving
                while ((transform.position - endpos).sqrMagnitude > 25f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, endpos, UnitStats.MoveSpeed * _chargeSpeedMultiplier * Time.deltaTime);
                    yield return null;
                }

                _chargeMeleeWeapon.EnableHitbox(false);
                (UnitVisuals as BossEnemyVisuals).ChargeAttackAnimationEnded();

                yield return new WaitForSeconds(_timeBetweenCharges);
            }


            CanMove = true;
            CanShoot = true;
            CanHit = true;
            CanChargeAttack = true;
        }
        #endregion

        #region JumpToPlayer
        [SerializeField, Space]
        private float _jumpHeight = 10f;
        [SerializeField]
        private float _jumpDuration = 2f;
        [SerializeField]
        private AnimationCurve _jumpCurve;
        [SerializeField]
        private MeleeWeapon _jumpMeleeWeapon;

        public void JumpToPlayer()
        {
            CanShoot = false;
            CanMove = false;
            CanHit = false;
            CanChargeAttack = false;
            StartCoroutine(JumpToPlayerCor());
        }
        private IEnumerator JumpToPlayerCor()
        {
            var startPos = transform.position;
            var endPos = Player.transform.position;
            (UnitVisuals as BossEnemyVisuals).JumpStart();
            
            float time = 0;
            while (time < _jumpDuration)
            {
                time += Time.deltaTime;
                Vector3 pos = Vector3.Lerp(startPos, endPos, time / _jumpDuration);
                pos.y += _jumpCurve.Evaluate(time / _jumpDuration) * _jumpHeight;
                transform.position = pos;
                yield return null;
            }
            (UnitVisuals as BossEnemyVisuals).JumpEnd();
            CanShoot = true;
            CanMove = true;
            CanHit = true;
            CanChargeAttack = true;
            _jumpMeleeWeapon.EnableHitbox(true);
            yield return new WaitForSeconds(0.3f);
            _jumpMeleeWeapon.EnableHitbox(false);
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