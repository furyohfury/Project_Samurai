using UnityEngine;
using UnityEngine.Pool;
using Zenject;
namespace Samurai
{
    public class DefaultPlayerGunPool : MonoBehaviour
    {
        [Inject]
        private DefaultPlayerWeaponProjectile.Factory _factory;

        public enum PoolType
        {
            Stack,
            LinkedList
        }

        public PoolType poolType;

        // Collection checks will throw errors if we try to release an item that is already in the pool.
        public bool collectionChecks = true;
        public int maxPoolSize = 10;

        IObjectPool<DefaultPlayerWeaponProjectile> m_Pool;

        public IObjectPool<DefaultPlayerWeaponProjectile> Pool
        {
            get
            {
                if (m_Pool == null)
                {
                    if (poolType == PoolType.Stack)
                        m_Pool = new ObjectPool<DefaultPlayerWeaponProjectile>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 5, maxPoolSize);
                    else
                        m_Pool = new LinkedPool<DefaultPlayerWeaponProjectile>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
                }
                return m_Pool;
            }
        }



        DefaultPlayerWeaponProjectile CreatePooledItem()
        {
            // GameObject proj = Instantiate(ProjectilePrefab);
            GameObject proj = _factory.Create().gameObject;
            return proj.GetComponent<DefaultPlayerWeaponProjectile>();
        }

        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(DefaultPlayerWeaponProjectile proj)
        {
            proj.OnReturnedToPool();
            proj.gameObject.SetActive(false);
            proj.transform.parent = this.transform;
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(DefaultPlayerWeaponProjectile proj)
        {
            proj.gameObject.SetActive(true);
            proj.transform.parent = null;
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(DefaultPlayerWeaponProjectile proj)
        {
            Destroy(proj.gameObject);
        }
    }
}