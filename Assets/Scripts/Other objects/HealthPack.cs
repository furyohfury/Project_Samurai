using UnityEngine;
namespace Samurai
{
    public class HealthPack : MonoBehaviour
    {
        [SerializeField]
        private int _health;
        public int Health {get => _health; private set => _health = value; }
    }
}