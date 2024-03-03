using Zenject;

namespace Samurai
{
    public class MinigunProjectile : Projectile
    {
        [Inject]
        public void Construct(ProjectileManager pmanager)
        {
            ProjectileManager = pmanager;
        }
        public class Factory : PlaceholderFactory<MinigunProjectile> { }
    }
}