using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class Testing_PoseImageDictionary : MonoBehaviour
    {
        public PoseImageDictionary poseImageDictionary;

        public void Add(PoseVoxel voxel, Texture2D tex)
        {
            poseImageDictionary.Add(voxel, tex);
        }
    }
}