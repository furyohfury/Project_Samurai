using System.Collections;
using UnityEngine;

namespace Samurai
{
    public abstract class ColorObject : MonoBehaviour
    {
        [SerializeField]
        private PhaseColor _currentColor;
        public PhaseColor CurrentColor { get => _currentColor; protected set => _currentColor = value; }

        [SerializeField]
        protected Renderer[] MeshForColorChange;

        [SerializeField]
        protected MaterialColorDictionary MaterialColorsDict;
        
        protected virtual void Start()
        {
            // For painting unit at spawn
            ChangeColor(CurrentColor);
        }
        [ContextMenu("Set Default Material Dictionary")]
        private void SetDefaultMaterialDictionary()
        {
            MaterialColorsDict = new MaterialColorDictionary() { { PhaseColor.Blue, Resources.Load<Material>("Materials/BlueColor") }, { PhaseColor.Red, Resources.Load<Material>("Materials/RedColor") }, { PhaseColor.Damaged, Resources.Load<Material>("Materials/DamagedColor") } };
        }
        public virtual void ChangeColor(PhaseColor color)
        {
            CurrentColor = color;
            foreach (var mesh in MeshForColorChange)
            {
                mesh.material = MaterialColorsDict[color];
            }
        }
    }
}