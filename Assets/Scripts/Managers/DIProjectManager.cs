using System.Collections;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class DIProjectManager : MonoInstaller
    {
        [SerializeField]
        private SaveLoadManager _saveLoadManager;        


        public override void InstallBindings()
        {
            Container.BindInstance(_saveLoadManager).AsSingle().NonLazy();
            
        }
    }
}