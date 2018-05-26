using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class Pose
    {
        #region Fields
        private Vector3 pos_m;
        private Quaternion rot_m;
        #endregion

        #region Methods
        #region Initialization
        public Pose(Camera cam)
            : this(cam.transform.position, cam.transform.rotation)
        { }

        public Pose(Vector3 worldPosition, Quaternion worldRotation)
        {
            pos_m = worldPosition;
            rot_m = worldRotation;
        }
        #endregion
        
        public bool Matches(Camera cam)
        {
            return Matches(cam.transform);
        }

        public bool Matches(Transform transform)
        {
            return Matches(transform.position, transform.rotation);
        }

        public bool Matches(Vector3 worldPosition, Quaternion worldRotation, float xLeeway = 0, float yLeeway = 0, float zLeeway = 0)
        {
            if(pos_m != null
                && rot_m != null)
            {
                bool matches =
                    fallsInLeeway(worldPosition.x, xLeeway, pos_m.x)
                    && fallsInLeeway(worldPosition.y, yLeeway, pos_m.y)
                    && fallsInLeeway(worldPosition.z, zLeeway, pos_m.z)
                    && worldRotation == rot_m;

                return matches;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            Pose target = (Pose)obj;
            Vector3 otherPos = target.Position;
            Quaternion otherRot = target.Rotation;

            return Position.Equals(otherPos) && Rotation.Equals(otherRot);
        }

        #region Helpers
        private bool fallsInLeeway(float x, float leeway, float value)
        {
            return
                x + leeway >= value
                && x - leeway <= value;
        }
        #endregion
        #endregion

        #region Properties
        public Vector3 Position
        {
            get
            {
                return pos_m;
            }
        }
        public Quaternion Rotation
        {
            get
            {
                return rot_m;
            }
        }
        #endregion
    }
}