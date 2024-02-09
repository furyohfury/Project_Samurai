using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samurai
{
    public class ProjectileManager : MonoBehaviour
    {
        public static ProjectileManager Instance;

        public List<Projectile> ProjectileList;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);

            ProjectileList = new();
        }
        private void Update()
        {
            foreach (var proj in ProjectileList)
            {
                proj.transform.position += proj.GetProjectileStats().ProjectileSpeed * Time.deltaTime * proj.transform.forward;
            }
        }
    }
}