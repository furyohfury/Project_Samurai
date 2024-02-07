using System.Collections;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class GameManager : MonoInstaller
    {
        public override void InstallBindings()
        {
            var defaultPlayerGunPool = FindObjectOfType<DefaultPlayerGunPool>();
            Container.BindInstance(defaultPlayerGunPool).AsSingle();
            Container.BindInstance(FindObjectOfType<Player>()).AsSingle();
        }
    }
}