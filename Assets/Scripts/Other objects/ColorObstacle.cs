using UnityEngine;
using Zenject;
namespace Samurai
{
    public class ColorObstacle: ColorObject
    {
        [Inject]
        private Player Player;
        [Inject]
        private DefaultPlayerGunPool _defaultPlayerGunPool;


        private void OnEnable()
        {
            Player.OnPlayerSwapColor += ChangeColor;
        }
        private void OnDisable()
        {
            Player.OnPlayerSwapColor -= ChangeColor;
        }        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Projectile proj) && proj.CurrentColor != this.CurrentColor)
            {
                var projDef = proj as DefaultPlayerWeaponProjectile;
                if (projDef != null) _defaultPlayerGunPool.Pool.Release(projDef);
                else Destroy(proj.gameObject);
            }
        }        
    }
}