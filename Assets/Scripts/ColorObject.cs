using System.Collections;
using UnityEngine;

namespace Samurai
{
    public abstract class ColorObject : MonoBehaviour
    {

        private PhaseColor _currentColor;
        public PhaseColor CurrentColor { get => _currentColor; protected set => _currentColor = value; }

        [SerializeField]
        protected MeshRenderer MeshForColorChange;

        public abstract void ChangeColor(PhaseColor color);
    }
}