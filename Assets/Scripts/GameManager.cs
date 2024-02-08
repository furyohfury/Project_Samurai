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
        }
    }
}