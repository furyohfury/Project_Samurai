using System.Collections;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class DIProjectManager : MonoInstaller
    {
        [SerializeField]
        private SaveLoadManager _saveLoadManager;
        [SerializeField]
        private RuntimeObjectsCreator _runtimeObjectsCreator;        

        [SerializeField, Space]
        private GameObject _defaultPlayerWeaponPrefab;
        [SerializeField]
        private GameObject _shotgunPrefab;
        [SerializeField]
        private GameObject _riflePrefab;
        [SerializeField]
        private GameObject _minigunPrefab;


        public override void InstallBindings()
        {
            Container.BindInstance(_saveLoadManager).AsSingle().NonLazy();
            Container.BindInstance(_runtimeObjectsCreator).AsSingle().NonLazy();

            // Factories            

            Container.BindFactory<DefaultPlayerWeapon, DefaultPlayerWeapon.Factory>().FromComponentInNewPrefab(_defaultPlayerWeaponPrefab);
            Container.BindFactory<Shotgun, Shotgun.Factory>().FromComponentInNewPrefab(_shotgunPrefab);            
            Container.BindFactory<Rifle, Rifle.Factory>().FromComponentInNewPrefab(_riflePrefab);
            Container.BindFactory<Minigun, Minigun.Factory>().FromComponentInNewPrefab(_minigunPrefab);
        }
    }
}