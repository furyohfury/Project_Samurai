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
        [ContextMenu("Get all mesh from children")]
        public void GetAllMeshFromChildren()
        {
            MeshForColorChange = GetComponentsInChildren<Renderer>();
        }

        [SerializeField]
        protected MaterialColorDictionary MaterialColorsDict;
        
        protected virtual void Start()
        {
            if (MaterialColorsDict  == null) SetDefaultMaterialDictionary();
            // For painting unit at spawn
            ChangeColor(CurrentColor);
        }
        [ContextMenu("Set Default Material Dictionary")]
        private void SetDefaultMaterialDictionary()
        {
            MaterialColorsDict = new MaterialColorDictionary() { { PhaseColor.Blue, Resources.Load<Material>("Materials/BlueColor") }, { PhaseColor.Red, Resources.Load<Material>("Materials/RedColor") }, { PhaseColor.Damaged, Resources.Load<Material>("Materials/DamagedColor") } };
        }
        public virtual void ChangeColorVisual(PhaseColor color)
        {
            foreach (var mesh in MeshForColorChange)
            {
                // mesh.material = MaterialColorsDict[color];
                mesh.material.color = PhaseColorToColor(color);
            }
        }
        public virtual void ChangeCurrentColor(PhaseColor color) => CurrentColor = color;
        public virtual void ChangeColor(PhaseColor color)
        {
            ChangeCurrentColor(color);
            ChangeColorVisual(color);
        }

        private Color PhaseColorToColor(PhaseColor color)
        {
            switch (color)
            {
                case PhaseColor.Blue:
                    return Color.blue;
                case PhaseColor.Red:
                    return Color.red;
                case PhaseColor.Damaged:
                    return Color.yellow;
                case PhaseColor.Green:
                    return Color.green;
                case PhaseColor.Default:
                    return Color.white;
            }
            return Color.white;
        }
    }
}