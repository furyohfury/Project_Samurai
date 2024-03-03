using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

namespace Samurai
{
    public class Tests : MonoBehaviour
    {
        public string EncodedString;
        private void Start()
        {
            // SaveLoadManager.Test(EncodedString);
            // Debug.Log(Application.persistentDataPath);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Tests))]
    public class TestCustomEditor : Editor
    {
        private SerializedProperty _encString;

        private Tests _target;
        private void OnEnable()
        {
            _encString = serializedObject.FindProperty("EncodedString");
            _target = (Tests)target;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_encString);

            if (GUILayout.Button("Decode string"))
            {
                // SaveLoadManager.Test(_encString.ToString());
            }

            if (GUILayout.Button("LoadScene"))
            {
                SaveLoadManager.LoadLastSave();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif 

}
