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
        private DefaultPlayerGunPool DefaultPlayerGunPool;


        [SerializeField, Space]
        private ProjectileManager ProjectileManager;
        [SerializeField]
        private SaveLoadSceneAssistant SaveLoadSceneAssistant;

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

            // UI
            Container.BindInstance(PlayerUI).AsSingle();
            // Container.BindInstance(MainMenuUI).AsSingle();
            Container.BindInstance(SettingsMenuUI).AsSingle();
            Container.BindInstance(PauseMenu).AsSingle();
            Container.BindInstance(LoseMenu).AsSingle();

            // Camera related
            Container.BindInstance(Camera).AsSingle();

            // Other
            Container.Bind<Collider>().FromComponentSibling();
        }
    }
}