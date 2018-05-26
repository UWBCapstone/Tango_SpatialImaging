using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public static class EulerClamper
    {
        public static float ClampEuler(float euler)
        {
            if (euler < 0)
            {
                euler += 360;
            }

            return euler % 360;
        }

        public static Vector3 ClampEulerVector(Vector3 eulers)
        {
            return new Vector3(ClampEuler(eulers.x), ClampEuler(eulers.y), ClampEuler(eulers.z));
        }
    }
}