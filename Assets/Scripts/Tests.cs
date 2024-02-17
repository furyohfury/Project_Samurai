using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace Samurai
{
    public class Tests : MonoBehaviour
    {
        [SerializeField, Range(0,1f)]
        private float _timescale = 1;
        private bool _setTimeScale = false;
        private void Update()
        {
            Time.timeScale = _timescale;
        }
    }
}
