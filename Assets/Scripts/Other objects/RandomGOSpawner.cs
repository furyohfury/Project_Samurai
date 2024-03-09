using System.Collections;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class RandomGOSpawner : MonoBehaviour
    {
        [Inject]
        private RuntimeObjectsCreator _runtimeObjectsCreator;

        [SerializeField]
        private GameObject[] objectPrefabs; // Array of game object prefabs to spawn
        [SerializeField]
        private Transform[] spawnPoints;    // Array of spawn points
        [SerializeField, Range(0, 100f)]
        private float minSpawnTime = 1f;    // Minimum time between spawns
        [SerializeField, Range(0, 200f)]
        private float maxSpawnTime = 3f;    // Maximum time between spawns

        private void Start()
        {
            // Start spawning coroutine
            StartCoroutine(SpawnObjects());
        }

        private IEnumerator SpawnObjects()
        {
            while (true)
            {
                // Wait for random time within the specified range
                yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

                // Get a random spawn point index
                int spawnPointIndex = Random.Range(0, spawnPoints.Length);

                // Get a random object prefab
                GameObject objectToSpawn = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

                // Instantiate the object at the random spawn point
                if (objectToSpawn.TryGetComponent(out RangeWeapon _)) _runtimeObjectsCreator.CreateWeapon(objectToSpawn.name);
                else Instantiate(objectToSpawn, spawnPoints[spawnPointIndex].position, Quaternion.identity);
            }
        }
    }
}