using UnityEngine;
using Zenject;
namespace Samurai
{
    public class ShotgunProjectile : Projectile
    {
        
        public class Factory : PlaceholderFactory<ShotgunProjectile> { }
    }
}