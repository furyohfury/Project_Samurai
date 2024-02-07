using System.Collections;
using UnityEngine;

namespace Samurai
{
    public abstract class ColorObject : MonoBehaviour
    {

        private PhaseColor _currentColor;
        public PhaseColor CurrentColor { get => _currentColor; protected set => _currentColor = value; }

        [SerializeField]
        protected MeshRenderer[] MeshForColorChange;

        [SerializeField]
        protected MaterialColorDictionary MaterialColorsDict<PhaseColor, Material>;

        public virtual void ChangeColor(PhaseColor color)
        {
            CurrentColor = color;
            foreach (var mesh in MeshForColorChange)
            {
                mesh.material = MaterialColorDictionary[color];
            }
        }
    }
}