using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System; // IntPtr

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox
{
    public class PoseImageCreator : MonoBehaviour
    {
        #region Fields
        public TangoApplication tangoManager;
        #endregion

        #region Methods
        #region Initialization
        public void Start()
        {
            if (!failsafeSetTangoManager())
            {
                Debug.LogError("PoseImageCreator: Tango manager not found in scene!");
            }

            register();
        }

        // ERROR TESTING - UNNECESSARY, REMOVE
        private void register()
        {
            tangoManager.Register(this);
        }

        private bool failsafeSetTangoManager()
        {
            if (tangoManager != null)
            {
                return true;
            }
            else
            {
                tangoManager = GameObject.FindObjectOfType<TangoApplication>();
                return tangoManager != null;
            }
        }

        public Texture2D MakeCameraImage()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log("PoseImageCreator: Making image from camera image...");

            IntPtr textureId = new IntPtr((int)getARTexturePointer()); // GetCameraTextureID from videooverlayprovider?
            Texture2D cameraTexture = Texture2D.CreateExternalTexture(
                1080,
                1920,
                TextureFormat.RGBA32,
                false,
                false,
                textureId);

            return cameraTexture;
#else
            return null;
#endif
        }

        public uint getARTexturePointer()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log("PoseImageCreator: Attempting to get AR Texture handle now...");

            uint tex = PublicTangoAPI.TangoUnity_getArTexture();

            Debug.Log("PoseImageCreator: Handle = " + tex);

            return tex;
#else
            return 0;
#endif
        }
#endregion
#endregion
    }
}
#endif