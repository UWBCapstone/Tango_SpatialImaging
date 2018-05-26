using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class Testing_PoseVoxelSet : MonoBehaviour
    {
        public PoseVoxelSet poseVoxelSet;

        public bool AddedIsSameAsStored(Pose pose)
        {
            // Get the current pose
            // Turn it into a Voxel
            // Return it

            if (poseVoxelSet.AddFor(pose))
            {
                PoseVoxel voxel = poseVoxelSet.ToPoseVoxel(pose);
                PoseVoxel storedVoxel = poseVoxelSet[pose];

                return voxel.Equals(storedVoxel);
            }

            return false;
        }

        public Pose GetCurrentPose()
        {
            return new Pose(Camera.main);
        }
    }
}