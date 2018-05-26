using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class PoseImageDictionary : MonoBehaviour
    {
        #region Fields
        public bool Debugging = false;
        private Dictionary<PoseVoxel, Texture2D> imageMap_m;

        #region Debugging Fields
        public Texture2D[] debuggingTextureArray_m;
        public Vector3[] debuggingMinPositionArray_m;
        public Vector3[] debuggingMaxPositionArray_m;
        public Vector3[] debuggingMinRotationArray_m;
        public Vector3[] debuggingMaxRotationArray_m;
        [HideInInspector]
        private const int debugTexArrSize = 30;
        private int debugArrIndex = 0;
        #endregion
        #endregion

        #region Methods
        #region Initialization
        public void Start()
        {
            imageMap_m = new Dictionary<PoseVoxel, Texture2D>();

            InitDebuggingTools();
        }
        #endregion

        /// <summary>
        /// Destroys the texture sent in, but creates a copy in the dictionary and returns the texture.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="tex"></param>
        public Texture2D Add(PoseVoxel key, Texture2D tex)
        {
            if (!imageMap_m.ContainsKey(key)) {
                if (tex != null) {
                    Texture2D copy = new Texture2D(tex.width, tex.height, tex.format, false);
                    Graphics.CopyTexture(tex, copy);
                    destroyTex(tex);

                    // Add to dictionary
                    imageMap_m.Add(key, copy);

                    // Update debugger
                    addToDebuggingTools(key.Min, key.Max, copy);
                    
                    return copy;
                }
            }

            return null;
        }

        public bool Remove(PoseVoxel key)
        {
            if (imageMap_m.ContainsKey(key))
            {
                Texture2D tex = imageMap_m[key];
                destroyTex(tex);

                // Update debugger
                ResetDebuggingTools();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Contains(PoseVoxel key)
        {
            return imageMap_m.ContainsKey(key);
        }

        public void Clear()
        {
            // Destroy all textures
            foreach (var t in imageMap_m.Values)
            {
                destroyTex(t);
            }

            imageMap_m.Clear();

            // Reset debugging tools
            ResetDebuggingTools();
        }

        public void OnDestroy()
        {
            Clear();
        }

        private void destroyTex(Texture2D tex)
        {
            if (tex != null)
            {
#if UNITY_EDITOR
                GameObject.DestroyImmediate(tex);
#else
            GameObject.Destroy(tex);
#endif
            }
        }

        #region Debugging
        public void InitDebuggingTools()
        {
            // For debugging purposes through the script's lifetime
            debuggingTextureArray_m = new Texture2D[debugTexArrSize];
            debuggingMinPositionArray_m = new Vector3[debugTexArrSize];
            debuggingMaxPositionArray_m = new Vector3[debugTexArrSize];
            debuggingMinRotationArray_m = new Vector3[debugTexArrSize];
            debuggingMaxRotationArray_m = new Vector3[debugTexArrSize];
        }

        public void ResetDebuggingTools()
        {
            clearDebuggingTools();
            foreach(var pair in imageMap_m)
            {
                Pose min = pair.Key.Min;
                Pose max = pair.Key.Max;
                Texture2D img = pair.Value;

                addToDebuggingTools(min, max, img);
            }
        }

        private void clearDebuggingTools()
        {
            debugArrIndex = 0;
            InitDebuggingTools();
        }

        private void addToDebuggingTools(Pose min, Pose max, Texture2D img)
        {
            int i = debugArrIndex;

            debuggingTextureArray_m[i] = img;
            debuggingMinPositionArray_m[i] = min.Position;
            debuggingMaxPositionArray_m[i] = max.Position;
            debuggingMinRotationArray_m[i] = min.Rotation.eulerAngles;
            debuggingMaxRotationArray_m[i] = max.Rotation.eulerAngles;

            ++debugArrIndex;
        } 
        #endregion
        #endregion

        #region Properties
        public Texture2D this[PoseVoxel key]
        {
            get
            {
                if (imageMap_m.ContainsKey(key))
                {
                    Texture2D orig = imageMap_m[key];
                    Texture2D copy = new Texture2D(orig.width, orig.height, orig.format, false);

                    Graphics.CopyTexture(orig, copy);

                    return copy;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
    }

}