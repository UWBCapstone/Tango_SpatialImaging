using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class PoseVoxelSet : MonoBehaviour
    {
        private enum MinMaxTypes
        {
            x,
            y,
            z,
            xRot,
            yRot,
            zRot
        };

        #region Fields
        public bool Debugging = false;
        private HashSet<PoseVoxel> poseVoxelSet_m;
        
        private const float xWidth = 0.25f;
        private const float yWidth = 0.25f;
        private const float zWidth = 0.25f;

        private const float xDegree = 90f;
        private const float yDegree = 90f;
        private const float zDegree = 90f;
        #endregion

        #region Methods
        #region Initialization
        public PoseVoxelSet()
        {
            poseVoxelSet_m = new HashSet<PoseVoxel>();

            Debug.Log("PoseVoxelSet: xWidth = " + xWidth + "; yWidth = " + yWidth + "; zWidth = " + zWidth + "; xDegree = " + xDegree + "; yDegree = " + yDegree + "; zDegree = " + zDegree);
        }
        #endregion

        public bool Add(PoseVoxel voxel)
        {
            if (!poseVoxelSet_m.Contains(voxel))
            {
                poseVoxelSet_m.Add(voxel);
                return true;
            }

            return false;
        }
        
        public bool AddFor(Pose pose)
        {
            // Determine if the pose is already accounted for
            if (this[pose] == null)
            {
                PoseVoxel voxel = ToPoseVoxel(pose);
                return Add(voxel);
            }

            return false;
        }

        public bool Remove(PoseVoxel voxel)
        {
            if (Contains(voxel))
            {
                poseVoxelSet_m.Remove(voxel);
                return true;
            }

            return false;
        }

        public bool Contains(PoseVoxel voxel)
        {
            return poseVoxelSet_m.Contains(voxel);
        }

        public PoseVoxel ToPoseVoxel(Pose pose)
        {
            // if the pose is not accounted for, calculate a pose voxel and add it in
            Vector3 pos = pose.Position;
            Vector3 eulers = pose.Rotation.eulerAngles;

            // Figure out the position ranges for the voxel
            Vector2 xMinMax = getMinMax(pos.x, MinMaxTypes.x);
            Vector2 yMinMax = getMinMax(pos.y, MinMaxTypes.y);
            Vector2 zMinMax = getMinMax(pos.z, MinMaxTypes.z);
            Vector3 minPos = new Vector3(xMinMax.x, yMinMax.x, zMinMax.x);
            Vector3 maxPos = new Vector3(xMinMax.y, yMinMax.y, zMinMax.y);

            // Figure out the rotation ranges for the voxel
            Vector2 xRotMinMax = getMinMax(eulers.x, MinMaxTypes.xRot);
            Vector2 yRotMinMax = getMinMax(eulers.y, MinMaxTypes.yRot);
            Vector2 zRotMinMax = getMinMax(eulers.z, MinMaxTypes.zRot);
            Vector3 minEulers = new Vector3(xRotMinMax.x, yRotMinMax.x, zRotMinMax.x);
            Vector3 maxEulers = new Vector3(xRotMinMax.y, yRotMinMax.y, zRotMinMax.y);

            PoseVoxel newVox = new PoseVoxel(minPos, maxPos, minEulers, maxEulers);

            return newVox;
        }

        #region Helpers
        private Vector2 getMinMax(float value, MinMaxTypes minMaxType)
        {
            Vector2 minMax = new Vector2(0, 0);
            
            float min = 0;
            float max = 0;
            float width = 0;
            switch (minMaxType)
            {
                case MinMaxTypes.x:
                    width = xWidth;
                    break;
                case MinMaxTypes.y:
                    width = yWidth;
                    break;
                case MinMaxTypes.z:
                    width = zWidth;
                    break;
                case MinMaxTypes.xRot:
                    width = xDegree;
                    break;
                case MinMaxTypes.yRot:
                    width = yDegree;
                    break;
                case MinMaxTypes.zRot:
                    width = zDegree;
                    break;
                default:
                    Debug.LogError("PoseVoxelSet: Encountered unknown min-max type! Please update the enum for this and the logic in this method.");
                    break;
            }

            if (width == 0)
            {
                return Vector2.zero;
            }
            
            min = (int)(value / width);
            max = min + 1;
            
            min *= width;
            max *= width;
            
            // Accommodate for rotation issues
            if (minMaxType == MinMaxTypes.xRot
                || minMaxType == MinMaxTypes.yRot
                || minMaxType == MinMaxTypes.zRot)
            {
                min = EulerClamper.ClampEuler(min);
                max = EulerClamper.ClampEuler(max);
            }

            minMax.x = min;
            minMax.y = max;
            
            return minMax;
        }

        private PoseVoxel getPoseVoxelFromSetFor(Pose pose)
        {
            foreach(var voxel in poseVoxelSet_m)
            {
                if (voxel.Contains(pose))
                {
                    return voxel;
                }
            }

            return null;
        }
        #endregion
        #endregion

        #region Properties
        public PoseVoxel this[Pose pose]
        {
            get
            {
                return getPoseVoxelFromSetFor(pose);
            }
        }
        public int Length
        {
            get
            {
                return poseVoxelSet_m.Count;
            }
        }
        public HashSet<PoseVoxel> Items
        {
            get
            {
                return new HashSet<PoseVoxel>(poseVoxelSet_m);
            }
        }
        #endregion
    }
}