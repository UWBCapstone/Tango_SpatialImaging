using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace CloakingBox
{
    [CustomEditor(typeof(PoseImageDictionary))]
    public class PoseImageDictionaryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PoseImageDictionary map = (PoseImageDictionary)target;
            map.Debugging = GUILayout.Toggle(map.Debugging, "Debugging");

            //DrawDefaultInspector();
            DisplayDebuggingInfo(map);
        }

        private void DisplayDebuggingInfo(PoseImageDictionary map)
        {
            //using (new EditorGUI.DisabledScope(map.Debugging == false))
            if(map.Debugging)
            {
                if (map.debuggingTextureArray_m != null)
                {
                    int size = map.debuggingTextureArray_m.Length;

                    // Display voxel information
                    EditorGUILayout.LabelField("Debugging Info:");
                    for (int i = 0; i < size; i++)
                    {
                        DisplayVoxel(map, i);
                    }
                }
            }
        }

        private void DisplayVoxel(PoseImageDictionary map, int index)
        {
            int oldIndentLevel = EditorGUI.indentLevel;

            // Display each voxel
            EditorGUI.indentLevel = 2;
            EditorGUILayout.LabelField("Voxel [" + index.ToString() + "]:");
            EditorGUILayout.ObjectField("Icon", map.debuggingTextureArray_m[index], typeof(Texture2D), false);
            EditorGUILayout.Vector3Field("Min Pos", map.debuggingMinPositionArray_m[index]);
            EditorGUILayout.Vector3Field("Max Pos", map.debuggingMaxPositionArray_m[index]);
            EditorGUILayout.Vector3Field("Min Euler", map.debuggingMinRotationArray_m[index]);
            EditorGUILayout.Vector3Field("Max Euler", map.debuggingMaxRotationArray_m[index]);

            EditorGUI.indentLevel = oldIndentLevel;
        }
    }
}