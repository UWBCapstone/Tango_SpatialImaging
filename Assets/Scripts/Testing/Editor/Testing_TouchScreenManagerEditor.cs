using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace CloakingBox
{
    [CustomEditor(typeof(Testing_TouchScreenManager))]
    public class Testing_TouchScreenManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Testing_TouchScreenManager manager = (Testing_TouchScreenManager)target;
            if(GUILayout.Button("Simulate Tap"))
            {
                manager.OnTap();
            }
        }
    }
}