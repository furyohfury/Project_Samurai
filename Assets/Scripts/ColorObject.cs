using System.Collections;
using UnityEngine;

namespace Samurai
{
    public class ColorObject : MonoBehaviour
    {

        private PhaseColor _currentColor;
        public PhaseColor CurrentColor { get => _currentColor; protected set => _currentColor = value; }
    }
}