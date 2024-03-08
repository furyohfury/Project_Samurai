using MoreMountains.Feedbacks;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using Zenject;
namespace Samurai
{
    public abstract class RangeWeapon : MonoBehaviour
    {

        [SerializeField]
        protected int _numberOfBulletsForPlayer = int.MaxValue;
        public int NumberOfBulletsForPlayer { get => _numberOfBulletsForPlayer; protected set => _numberOfBulletsForPlayer = value; }

        [SerializeField]
        protected float EnemyShootingDelay;
        [SerializeField]
        protected float PlayerShootingDelay;

        private bool _canShoot = true;
        public bool CanShoot { get => _canShoot; set => _canShoot = value; }

        // [SerializeField]
        // protected GameObject WeaponProjectilePrefab;

        [SerializeField]
        protected bool _isPickable = true;
        public bool IsPickable
        {
            get => _isPickable;
            protected set => _isPickable = value;
        }

        public abstract Vector3 WeaponPositionWhenPicked { get; }
        public abstract Vector3 WeaponRotationWhenPicked { get; }
        protected Quaternion DefaultRotation = Quaternion.Euler(new Vector3(0, 0, 90));

        public Unit Owner { get; protected set; }

        [SerializeField]
        protected ProjectileStatsStruct ProjectileStats;
        public ProjectileStatsStruct GetProjectileStats() => ProjectileStats;

        [SerializeField]
        protected ProjectileStatsStruct PlayerProjectileStats;
        [SerializeField]
        protected ProjectileStatsStruct EnemyProjectileStats;

        [SerializeField]
        protected AnimatorController _animController;
        public AnimatorController AnimController
        {
            get => _animController;
            protected set => _animController = value;
        }
        [SerializeField]
        protected Renderer[] _mesh;
        [ContextMenu("Get all mesh from children")]
        public void GetAllMeshFromChildren()
        {
            _mesh = GetComponentsInChildren<Renderer>();
        }

        [SerializeField, Space]
        protected MMFeedbacks ShootingFeedbacks;


        #region Unity_Methods
        protected void Awake()
        {
            if (_mesh == null) _mesh = GetComponentsInChildren<Renderer>();
        }
        protected virtual void OnEnable()
        {
            Equipped(GetComponentInParent<Unit>());
            if (Owner is RangeEnemy rEnemy) rEnemy.OnDroppedWeapon += Dropped;
        }
        protected virtual void OnDisable()
        {
            if (Owner is RangeEnemy rEnemy) rEnemy.OnDroppedWeapon -= Dropped;
        }
        #endregion

        /// <summary>
        /// Abstract
        /// </summary>
        public abstract void RangeAttack();

        public virtual void Equipped(Unit owner)
        {            
            Owner = owner;
            if (owner == null) return;

            if (owner is Player) ProjectileStats = PlayerProjectileStats;
            else ProjectileStats = EnemyProjectileStats;

            if (owner is Player) transform.SetLocalPositionAndRotation(WeaponPositionWhenPicked, Quaternion.Euler(WeaponRotationWhenPicked));

            if (this is IRangePickableWeapon pickable) pickable.GlowingFeedback?.StopFeedbacks();
        }
        /// <summary>
        /// Must be last in RangeAttack()
        /// </summary>
        protected virtual void CheckIfEmpty()
        {
            if (Owner as Player != null)
            {
                NumberOfBulletsForPlayer -= 1;
                if (NumberOfBulletsForPlayer <= 0)
                {
                    OnBulletsEnded?.Invoke();
                }
            }
        }
        protected void SetShootingDelay()
        {
            if (!CanShoot) return;
            StartCoroutine(ShootDelayCoroutine(Owner as Player == null ? EnemyShootingDelay : PlayerShootingDelay)); 
        }
        protected IEnumerator ShootDelayCoroutine(float delay)
        {
            CanShoot = false;
            yield return new WaitForSeconds(delay);
            CanShoot = true;
        }
        protected void Dropped()
        {
            Owner = null;
            this.transform.rotation = DefaultRotation;

            // Raycast floor. Floor layer is 6
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 20f, 1 << Constants.FloorLayer))
            {
                transform.parent = null;
                transform.position = hit.point;

                if (this is IRangePickableWeapon pickable) pickable.GlowingFeedback?.PlayFeedbacks();
            }
            else
            {
                this.enabled = false;
                Destroy(gameObject);
            }
        }

        public void MeshVisible(bool isEnabled)
        {
            foreach (var mesh in _mesh) mesh.enabled = isEnabled;
        }

        public void ApplyBuff(ProjectileStatsStruct stats)
        {
            ProjectileStats.Damage += stats.Damage;
        }
        public void ApplyBuff(int numberOfBullets)
        {
            NumberOfBulletsForPlayer = numberOfBullets;
        }


        public SimpleHandle OnBulletsEnded;
    }
}