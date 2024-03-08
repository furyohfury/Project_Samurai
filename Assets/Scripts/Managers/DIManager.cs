using MoreMountains.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Samurai
{
    public class DIManager : MonoInstaller
    {
        [SerializeField]
        private Player Player;
        /* [SerializeField]
        private PlayerVisuals PlayerVisuals;
        [SerializeField]
        private PlayerPhysics PlayerPhysics;
        [SerializeField]
        private PlayerInput PlayerInput; */
        [SerializeField, Space]
        private GameObject _defaultPlayerWeaponPrefab;
        [SerializeField]
        private GameObject _shotgunPrefab;
        [SerializeField]
        private GameObject _riflePrefab;
        [SerializeField]
        private GameObject _minigunPrefab;


        [SerializeField, Space]
        private GameObject _defPlayerWeaponProjPrefab;
        [SerializeField]
        private GameObject _shotgunProjectilePrefab;
        [SerializeField]
        private GameObject _rifleProjectilePrefab;
        [SerializeField]
        private GameObject _minigunProjectilePrefab;

        [SerializeField, Space]
        private DefaultPlayerGunPool DefaultPlayerGunPool;

        [SerializeField, Space]
        private ProjectileManager ProjectileManager;
        [SerializeField]
        private SaveLoadSceneAssistant SaveLoadSceneAssistant;
        [SerializeField]
        private RuntimeObjectsCreator _runtimeObjectsCreator;

        [SerializeField, Space]
        private PlayerUI PlayerUI;
        // [SerializeField]
        // private MainMenu MainMenuUI;
        [SerializeField]
        private SettingsMenu SettingsMenuUI;
        [SerializeField]
        private PauseMenu PauseMenu;
        [SerializeField]
        private LoseMenu LoseMenu;

        [SerializeField, Space]
        private Camera Camera;
        public override void InstallBindings()
        {
            // Player
            Container.BindInstance(Player).AsSingle();

            // Units
            Container.Bind<Unit>().FromComponentSibling();
            Container.Bind<UnitVisuals>().FromComponentSibling();
            Container.Bind<UnitPhysics>().FromComponentSibling();
            Container.Bind<UnitInput>().FromComponentSibling();
            Container.Bind<Animator>().FromComponentSibling();
            Container.Bind<MMHealthBar>().FromComponentSibling().WhenInjectedInto<Unit>();

            // Enemies
            Container.Bind<CapsuleCollider>().FromComponentSibling().WhenInjectedInto<EnemyPhysics>();
            Container.Bind<NavMeshAgent>().FromComponentSibling();

            // Pools
            Container.BindInstance(DefaultPlayerGunPool).AsSingle();
            Container.Bind<EnemyPool>().FromComponentInChildren();
            Container.Bind<AIManager>().FromComponentInChildren();

            // Scene managers
            Container.BindInstance(ProjectileManager).AsSingle();
            Container.BindInstance(SaveLoadSceneAssistant).AsSingle();
            Container.BindInstance(_runtimeObjectsCreator).AsSingle().NonLazy();

            // UI
            Container.BindInstance(PlayerUI).AsSingle();
            // Container.BindInstance(MainMenuUI).AsSingle();
            Container.BindInstance(SettingsMenuUI).AsSingle();
            Container.BindInstance(PauseMenu).AsSingle();
            Container.BindInstance(LoseMenu).AsSingle();

            // Camera related
            Container.BindInstance(Camera).AsSingle();

            

            // Factories           
            Container.BindFactory<DefaultPlayerWeapon, DefaultPlayerWeapon.Factory>().FromComponentInNewPrefab(_defaultPlayerWeaponPrefab);
            Container.BindFactory<Shotgun, Shotgun.Factory>().FromComponentInNewPrefab(_shotgunPrefab);
            Container.BindFactory<Rifle, Rifle.Factory>().FromComponentInNewPrefab(_riflePrefab);
            Container.BindFactory<Minigun, Minigun.Factory>().FromComponentInNewPrefab(_minigunPrefab);

            Container.BindFactory<DefaultPlayerWeaponProjectile, DefaultPlayerWeaponProjectile.Factory>().FromComponentInNewPrefab(_defPlayerWeaponProjPrefab);
            Container.BindFactory<ShotgunProjectile, ShotgunProjectile.Factory>().FromComponentInNewPrefab(_shotgunProjectilePrefab);
            Container.BindFactory<RifleProjectile, RifleProjectile.Factory>().FromComponentInNewPrefab(_rifleProjectilePrefab);
            Container.BindFactory<MinigunProjectile, MinigunProjectile.Factory>().FromComponentInNewPrefab(_minigunProjectilePrefab);

            // Other
            Container.Bind<Collider>().FromComponentSibling();
        }
    }
}