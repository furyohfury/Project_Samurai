﻿using System.Collections;
using UnityEngine;
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
        [SerializeField]
        private GameObject _defPlayerWeaponProjPrefab;
        [SerializeField]
        private GameObject _shotgunProjectilePrefab;
        [SerializeField]
        private GameObject _rifleProjectilePrefab;
        [SerializeField]
        private GameObject _minigunProjectilePrefab;


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
            Container.Bind<UnitAnimator>().FromComponentSibling();

            // Enemies
            Container.Bind<Collider>().FromComponentSibling();
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

            // Factories
            Container.BindFactory<DefaultPlayerWeaponProjectile, DefaultPlayerWeaponProjectile.Factory>().FromComponentInNewPrefab(_defPlayerWeaponProjPrefab);
            Container.BindFactory<ShotgunProjectile, ShotgunProjectile.Factory>().FromComponentInNewPrefab(_shotgunProjectilePrefab);
            Container.BindFactory<RifleProjectile, RifleProjectile.Factory>().FromComponentInNewPrefab(_rifleProjectilePrefab);
            Container.BindFactory<MinigunProjectile, MinigunProjectile.Factory>().FromComponentInNewPrefab(_minigunProjectilePrefab);
        }
    }
}