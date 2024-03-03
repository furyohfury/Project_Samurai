using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Samurai
{
    public class ProjectileManager : MonoBehaviour
    {
        [HideInInspector]
        public List<Projectile> ProjectileList = new();
        private void Update()
        {
            foreach (var proj in ProjectileList)
            {
                proj.transform.position += proj.GetProjectileStats().ProjectileSpeed * Time.deltaTime * proj.transform.forward;
            }
        }
    }
}