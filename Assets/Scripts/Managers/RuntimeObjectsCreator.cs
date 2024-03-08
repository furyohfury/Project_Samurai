using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Samurai
{
    public class RuntimeObjectsCreator : MonoBehaviour
    {
        /* [Inject]
        private DefaultPlayerWeaponProjectile.Factory DefaultPlayerWeaponProjectile;
        [Inject]
        private ShotgunProjectile.Factory ShotgunProjectile;
        [Inject]
        private RifleProjectile.Factory RifleProjectile;
        [Inject]
        private MinigunProjectile.Factory MinigunProjectile; */

        [Inject]
        private DefaultPlayerWeapon.Factory DefaultPlayerWeaponFactory;
        [Inject]
        private Shotgun.Factory ShotgunFactory;
        [Inject]
        private Rifle.Factory RifleFactory;
        [Inject]
        private Minigun.Factory MinigunFactory;

        // Dictionary<string, IPlaceholderFactory> dict = new();

        public RangeWeapon CreateWeapon(string rWeapon)
        {
            // string name = typeof(T).Name;
            return rWeapon switch
            {
                "DefaultPlayerWeapon" => DefaultPlayerWeaponFactory.Create(),
                "Shotgun" => ShotgunFactory.Create(),
                "Rifle" => RifleFactory.Create(),
                "Minigun" => MinigunFactory.Create(),
                _ => null,
            };
        }

        /*private void Start()
        {
              fields = GetType().GetFields();
            foreach(var field in fields)
            {
                if (!field.FieldType.Name.Contains(field.Name))
                {
                    Debug.LogError($"RuntineObjectsCreator has a naming problem with {field.Name}");
                }

                dict.Add(field.Name, (IPlaceholderFactory) field.GetValue(this));
            } 
        }*/
        /* public GameObject GetObject<T>()
        {
            return dict[typeof(T).Name].Create()
        } */


    }
}