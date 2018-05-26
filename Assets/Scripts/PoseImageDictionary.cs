using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class PoseImageDictionary : MonoBehaviour
    {
        #region Fields
        private Dictionary<PoseVoxel, Texture2D> imageFileNameMap_m;
        #endregion

        #region Methods
        #region Initialization
        public void Start()
        {
            imageFileNameMap_m = new Dictionary<PoseVoxel, Texture2D>();
        }
        #endregion

        /// <summary>
        /// Destroys the texture sent in, but creates a copy in the dictionary and returns the texture.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="tex"></param>
        public Texture2D Add(PoseVoxel key, Texture2D tex)
        {
            if (imageFileNameMap_m.ContainsKey(key)) {
                Texture2D copy = new Texture2D(tex.width, tex.height, tex.format, false);
                Graphics.CopyTexture(tex, copy);
                destroyTex(tex);

                // Add to dictionary
                imageFileNameMap_m.Add(key, copy);

                return copy;
            }

            return null;
        }

        public bool Remove(PoseVoxel key)
        {
            if (imageFileNameMap_m.ContainsKey(key))
            {
                Texture2D tex = imageFileNameMap_m[key];
                destroyTex(tex);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Contains(PoseVoxel key)
        {
            return imageFileNameMap_m.ContainsKey(key);
        }

        public void Clear()
        {
            // Destroy all textures
            foreach (var t in imageFileNameMap_m.Values)
            {
                destroyTex(t);
            }

            imageFileNameMap_m.Clear();
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
        #endregion

        #region Properties
        public Texture2D this[PoseVoxel key]
        {
            get
            {
                if (imageFileNameMap_m.ContainsKey(key))
                {
                    Texture2D orig = imageFileNameMap_m[key];
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