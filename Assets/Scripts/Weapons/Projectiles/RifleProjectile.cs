using System.Collections;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class RifleProjectile : Projectile
    {
        
        public class Factory : PlaceholderFactory<RifleProjectile> { }
    }
}