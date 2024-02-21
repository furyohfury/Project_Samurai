using System.Collections;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class GameManager : MonoInstaller
    {
        [SerializeField]
        private DefaultPlayerGunPool DefaultPlayerGunPool;
        [SerializeField]
        private Player Player;
        [SerializeField]
        private EnemyPool EnemyPool;
        [SerializeField]
        private ProjectileManager ProjectileManager;
        [SerializeField]
        private PlayerUI PlayerUI;
        [SerializeField]
        private Camera Camera;
        public override void InstallBindings()
        {          
            Container.BindInstance(DefaultPlayerGunPool).AsSingle();
            Container.BindInstance(Player).AsSingle();
            Container.BindInstance(EnemyPool).AsSingle();
            Container.BindInstance(ProjectileManager).AsSingle();
            Container.BindInstance(PlayerUI).AsSingle();
            Container.BindInstance(Camera).AsSingle();
        }
    }
}