using UnityEngine;
namespace Samurai
{
// This example spans a random number of ParticleSystems using a pool so that old systems can be reused.
public class PoolExample : MonoBehaviour
{
    [SerializeField]
    private GameObject ProjectilePrefab;

    public enum PoolType
    {
        Stack,
        LinkedList
    }

    public PoolType poolType;

    // Collection checks will throw errors if we try to release an item that is already in the pool.
    public bool collectionChecks = true;
    public int maxPoolSize = 20;

    IObjectPool<Projectile> m_Pool;

    public IObjectPool<Projectile> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                if (poolType == PoolType.Stack)
                    m_Pool = new ObjectPool<Projectile>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
                else
                    m_Pool = new LinkedPool<Projectile>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
            }
            return m_Pool;
        }
    }



    Projectile CreatePooledItem()
    {
    GameObject proj = Instantiate(ProjectilePrefab);
        return proj;
    }

    // Called when an item is returned to the pool using Release
    void OnReturnedToPool(Projectile proj)
    {
        proj.OnReturnedToPool();
        proj.gameObject.SetActive(false);
    }

    // Called when an item is taken from the pool using Get
    void OnTakeFromPool(Projectile proj)
    {
        proj.gameObject.SetActive(true);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    void OnDestroyPoolObject(Projectile proj)
    {
        Destroy(proj.gameObject);
    }    
}
}