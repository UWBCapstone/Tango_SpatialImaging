using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace CloakingBox
{
    [CustomEditor(typeof(PoseVoxelSet))]
    public class PoseVoxelSetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PoseVoxelSet set = (PoseVoxelSet)target;
            set.Debugging = GUILayout.Toggle(set.Debugging, "Debugging");

            //DrawDefaultInspector();
            DisplayDebuggingInfo(set);
        }

        private void DisplayDebuggingInfo(PoseVoxelSet set)
        {
            //using (new EditorGUI.DisabledScope(map.Debugging == false))
            if (set.Debugging)
            {
                foreach(var voxel in set.Items)
                {
                    DisplayVoxel(voxel);
                }
            }
        }

        private void DisplayVoxel(PoseVoxel voxel)
        {
            int oldIndentLevel = EditorGUI.indentLevel;

            // Display each voxel
            EditorGUI.indentLevel = 2;
            EditorGUILayout.LabelField("Voxel:");
            EditorGUILayout.Vector3Field("Min Pos", voxel.Min.Position);
            EditorGUILayout.Vector3Field("Max Pos", voxel.Max.Position);
            EditorGUILayout.Vector3Field("Min Euler", voxel.Min.Rotation.eulerAngles);
            EditorGUILayout.Vector3Field("Max Euler", voxel.Max.Rotation.eulerAngles);

            EditorGUI.indentLevel = oldIndentLevel;
        }
    }
}