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
            Container.BindInstance(Player).AsSingle();

            Container.Bind<UnitVisuals>().FromComponentSibling();
            Container.Bind<UnitPhysics>().FromComponentSibling();
            Container.Bind<UnitInput>().FromComponentSibling();

            Container.BindInstance(DefaultPlayerGunPool).AsSingle();
            // Container.BindInstance(EnemyPool).AsSingle();

            Container.BindInstance(ProjectileManager).AsSingle();
            Container.BindInstance(SaveLoadSceneAssistant).AsSingle();

            Container.BindInstance(PlayerUI).AsSingle();
            // Container.BindInstance(MainMenuUI).AsSingle();
            Container.BindInstance(SettingsMenuUI).AsSingle();
            Container.BindInstance(PauseMenu).AsSingle();
            Container.BindInstance(LoseMenu).AsSingle();

            Container.BindInstance(Camera).AsSingle();
        }
    }
}