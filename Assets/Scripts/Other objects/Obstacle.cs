using UnityEngine;
using Zenject;
namespace Samurai
{
    [RequireComponent(typeof(Rigidbody))]
    public class Obstacle : MonoBehaviour
    {
        [Inject]
        private DefaultPlayerGunPool _defaultPlayerGunPool;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Projectile proj))
            {
                var projDef = proj as DefaultPlayerWeaponProjectile;
                if (projDef != null) _defaultPlayerGunPool.Pool.Release(projDef);
                else Destroy(proj.gameObject);
            }
        }
    }
}