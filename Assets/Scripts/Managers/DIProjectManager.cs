using System.Collections;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class DIProjectManager : MonoInstaller
    {
        [SerializeField]
        private SaveLoadManager _saveLoadManager;

        [SerializeField, Space]
        private GameObject _defPlayerWeaponProjPrefab;
        [SerializeField]
        private GameObject _shotgunProjectilePrefab;
        [SerializeField]
        private GameObject _rifleProjectilePrefab;
        [SerializeField]
        private GameObject _minigunProjectilePrefab;

        public override void InstallBindings()
        {
            Container.BindInstance(_saveLoadManager).AsSingle().NonLazy();

            // Factories
            Container.BindFactory<DefaultPlayerWeaponProjectile, DefaultPlayerWeaponProjectile.Factory>().FromComponentInNewPrefab(_defPlayerWeaponProjPrefab);
            Container.BindFactory<ShotgunProjectile, ShotgunProjectile.Factory>().FromComponentInNewPrefab(_shotgunProjectilePrefab);
            Container.BindFactory<RifleProjectile, RifleProjectile.Factory>().FromComponentInNewPrefab(_rifleProjectilePrefab);
            Container.BindFactory<MinigunProjectile, MinigunProjectile.Factory>().FromComponentInNewPrefab(_minigunProjectilePrefab);
        }
    }
}