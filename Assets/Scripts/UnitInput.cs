using System.Collections;
using UnityEngine;

namespace Samurai
{
    [RequireComponent(typeof(Unit))]
    public abstract class UnitInput : MonoBehaviour
    {
        protected Vector3 _movement;
        public ref Vector3 MoveDirection => ref _movement;
    }
}