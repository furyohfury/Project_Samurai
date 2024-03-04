using UnityEngine;
using Zenject;
namespace Samurai
{
    public class ColorObstacle: ColorObject
    {
        [Inject]
        private readonly Player Player;
        [Inject]
        private readonly DefaultPlayerGunPool _defaultPlayerGunPool;

        /* [SerializeField]
        private bool _switchesColorWithPlayer = false;


        private void OnEnable()
        {
            if (_switchesColorWithPlayer) (Player.UnitVisuals as PlayerVisuals).OnPlayerSwapColor += ChangeColor;
        }
        private void OnDisable()
        {
            if (_switchesColorWithPlayer) (Player.UnitVisuals as PlayerVisuals).OnPlayerSwapColor -= ChangeColor;
        } */        
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