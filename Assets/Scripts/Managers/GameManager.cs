using System.Collections;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class GameManager : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInstance(FindObjectOfType<DefaultPlayerGunPool>()).AsSingle();
            Container.BindInstance(FindObjectOfType<Player>()).AsSingle();
            Container.BindInstance(FindObjectOfType<EnemyPool>()).AsSingle();
            Container.BindInstance(FindObjectOfType<ProjectileManager>()).AsSingle();
            Container.BindInstance(FindObjectOfType<PlayerUI>()).AsSingle();
            Container.BindInstance(FindObjectOfType<PlayerInput>()).AsSingle();
        }
    }
}