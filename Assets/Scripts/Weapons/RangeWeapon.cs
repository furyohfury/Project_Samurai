using MoreMountains.Feedbacks;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
namespace Samurai
{
    [RequireComponent(typeof(Unit))]
    public abstract class RangeWeapon : MonoBehaviour
    {
        [SerializeField]
        protected int _numberOfBulletsForPlayer = int.MaxValue;
        public int NumberOfBulletsForPlayer { get => _numberOfBulletsForPlayer; protected set => _numberOfBulletsForPlayer = value; }

        [SerializeField]
        protected float EnemyShootingDelay;
        [SerializeField]
        protected float PlayerShootingDelay;

        public bool CanShoot { get; set; } = true; // todo incapsulation

        [SerializeField]
        protected GameObject WeaponProjectilePrefab;

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

        [SerializeField, Space]
        protected MMFeedbacks ShootingFeedbacks;


        #region Unity_Methods
        protected void Awake()
        {
            _mesh = GetComponentsInChildren<Renderer>();
        }
        protected virtual void OnEnable()
        {
            Equipped(GetComponentInParent<Unit>());
            if (Owner is RangeEnemy) (Owner as RangeEnemy).OnDroppedWeapon += Dropped;
        }
        protected virtual void Start()
        {

        }
        protected virtual void OnDisable()
        {
            if (Owner is RangeEnemy) (Owner as RangeEnemy).OnDroppedWeapon -= Dropped;
        }
        #endregion
        public abstract void RangeAttack();

        public virtual void Equipped(Unit owner)
        {            
            Owner = owner;
            if (owner == null) return;
            if (owner.GetType() == typeof(Player)) ProjectileStats = PlayerProjectileStats;
            else ProjectileStats = EnemyProjectileStats;
        }

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
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 20f, 1 << 6))
            {
                transform.parent = null;
                transform.position = hit.point;
            }
            else Destroy(gameObject);
        }

        public void MeshVisible(bool isEnabled)
        {
            foreach (var mesh in _mesh) mesh.enabled = isEnabled;
        }


        public SimpleHandle OnBulletsEnded;
    }
}