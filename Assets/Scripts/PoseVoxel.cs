using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class PoseVoxel
    {
        #region Fields
        private Vector3 minPos_m;
        private Vector3 maxPos_m;
        private Vector3 minRot_m; // min rotation angles (degrees)
        private Vector3 maxRot_m; // max rotation angles (degrees)
        #endregion

        #region Methods
        public PoseVoxel(Vector3 minPosition, Vector3 maxPosition, Vector3 minEulerAngleRotation, Vector3 maxEulerAngleRotation)
        {
            minPos_m = minPosition;
            maxPos_m = maxPosition;
            minRot_m = EulerClamper.ClampEulerVector(minEulerAngleRotation);
            maxRot_m = EulerClamper.ClampEulerVector(maxEulerAngleRotation);
        }

        public bool Contains(Pose pose)
        {
            // Has to lie within min max
            // Has to lie within rotation boundaries

            return posContains(pose) && rotContains(pose);
        }

        public override bool Equals(object obj)
        {
            PoseVoxel target = (PoseVoxel)obj;

            return target.Min.Equals(Min) && target.Max.Equals(Max);
        }

        #region Helpers
        private bool posContains(Pose pose)
        {
            Vector3 pos = pose.Position;

            // Check if it is above the minimum point and below the minimum point
            // Have to account for situations where the pose points align with the min and max points (would result in a dot of 0 since the vectors turn into zeroes)
            Vector3 minDir = ((pos - minPos_m) == Vector3.zero) ? maxPos_m - minPos_m : pos - minPos_m;
            Vector3 maxDir = ((pos - maxPos_m) == Vector3.zero) ? minPos_m - maxPos_m : pos - maxPos_m;

            float dot = Vector3.Dot(minDir, maxDir); // -1 means perfectly in the center of the two points - out to 0
            // Must be less than 0 or its extending past the corner boundaries of the cube (gives a little leeway as it technically just checks for a spherical area

            return dot < 0;
        }

        private bool rotContains(Pose pose)
        {
            Vector3 eulers = pose.Rotation.eulerAngles;

            eulers = new Vector3(
                EulerClamper.ClampEuler(eulers.x), 
                EulerClamper.ClampEuler(eulers.y), 
                EulerClamper.ClampEuler(eulers.z));

            bool contained = true;

            // Prep repeated values
            float min = minRot_m.x;
            float max = maxRot_m.x;
            float value = eulers.x;
            
            for(int i = 0; i < 3; i++)
            {
                min = minRot_m[i];
                max = maxRot_m[i];
                value = eulers[i];

                if(isEasyRotComparison(min, max))
                {
                    contained = contained && easyRotContains(min, max, value);
                }
                else
                {
                    contained = contained && hardRotContains(min, max, value);
                }

                if (!contained)
                {
                    break;
                }
            }

            return contained;
        }
        
        private bool isEasyRotComparison(float min, float max)
        {
            return max > min;
        }

        private bool easyRotContains(float min, float max, float value)
        {
            return
                value >= min
                && value <= max;
        }

        private bool hardRotContains(float min, float max, float value)
        {
            value = EulerClamper.ClampEuler(value);

            return
                (value <= 360 && value >= min)
                || (value >= 0 && value <= max);
        }
        #endregion
        #endregion

        #region Properties
        public Pose Min
        {
            get
            {
                Quaternion q = new Quaternion();
                q.eulerAngles = minRot_m;
                Pose pose = new Pose(minPos_m, q);
                return pose;
            }
        }
        public Pose Max
        {
            get
            {
                Quaternion q = new Quaternion();
                q.eulerAngles = maxRot_m;
                Pose pose = new Pose(maxPos_m, q);
                return pose;
            }
        }
        #endregion
    }
}