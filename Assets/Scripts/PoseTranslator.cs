using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public static class PoseTranslator
    {
        public static Pose GetPose(GameObject go)
        {
            return GetPose(go.transform);
        }

        public static Pose GetPose(Transform transform)
        {
            return GetPose(transform.position, transform.rotation);
        }

        public static Pose GetPose(Vector3 position, Quaternion rotation)
        {
            return new Pose(position, rotation);
        }
    }
}