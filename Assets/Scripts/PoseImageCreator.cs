using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System; // IntPtr
using System.Runtime.InteropServices; // GCHandle

#if UNITY_ANDROID || UNITY_EDITOR
using Tango;

namespace CloakingBox
{
    public class PoseImageCreator : MonoBehaviour
    {
        #region Fields
        public TangoApplication tangoManager;
        public Camera editorRenderCamera;
        public byte[] imageBuffer;
        private int imageWidth = 0;
        private int imageHeight = 0;

        public System.Object lockObject = new System.Object();
        #endregion

        #region Methods
        #region Initialization
        public void Start()
        {
            if (!failsafeSetTangoManager())
            {
                Debug.LogError("PoseImageCreator: Tango manager not found in scene!");
            }

            //register();
        }

        //// ERROR TESTING - UNNECESSARY, REMOVE
        //private void register()
        //{
        //    tangoManager.Register(this);
        //    //VideoOverlayListener.RegisterOnTangoImageAvailable(OnTangoImageAvailableEventHandler);
        //    VideoOverlayProvider.SetCallback(TangoEnums.TangoCameraId.TANGO_CAMERA_COLOR, new VideoOverlayProvider.APIOnImageAvailable(_OnImageAvailable));

        //    Debug.Log("PoseImageCreator: Registration complete.");
        //}

        // Taken from VideoOverlayListener method of the same name

        /// <summary>
        /// Modified the Tango library call of _OnImageAvailable in VideoOverlayListener (or VideoOverlayProvider) to call this method so that I could have access to the image seen by the camera.
        /// </summary>
        /// <param name="callbackContext"></param>
        /// <param name="cameraId"></param>
        /// <param name="image"></param>
        /// <param name="cameraMetadata"></param>
        public void BufferCameraImage(IntPtr callbackContext, TangoEnums.TangoCameraId cameraId, ref TangoImage image, ref TangoCameraMetadata cameraMetadata)
        {
            lock (lockObject)
            {
                //Debug.Log("PoseImageCreator: Image available. Marshalling data from image...");

                int uvPlaneSize = image.m_planeSize2 + 1;
                imageBuffer = new byte[image.m_planeSize0 + uvPlaneSize];

                uint width = image.m_width;
                uint height = image.m_height;
                imageWidth = (int)width;
                imageHeight = (int)height;

                // Convert from YUV888 to NV21
                // Get Ys.
                Marshal.Copy(image.m_planeData0, imageBuffer, 0, image.m_planeSize0);

                // Size of UV plane is image.plane_2_size + 1, since Tango guarantees that YUV888 has UV interleaved.
                Marshal.Copy(image.m_planeData2, imageBuffer, image.m_planeSize0, uvPlaneSize);

                //Debug.Log("PoseImageCreator: Marshal complete. Image width = " + imageWidth + "; Image height = " + imageHeight + "; imageBuffer Length = " + imageBuffer.Length);
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Texture2D MakeCameraImage()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            //Debug.Log("PoseImageCreator: Making image from camera image...");

            //////IntPtr textureId = new IntPtr((int)getARTexturePointer()); // GetCameraTextureID from videooverlayprovider?
            ////IntPtr textureId = texHandle;
            ////Texture2D cameraTexture = Texture2D.CreateExternalTexture(
            ////    1080,
            ////    1920,
            ////    TextureFormat.RGBA32,
            ////    false,
            ////    false,
            ////    textureId);

            ////return cameraTexture;

            Texture2D cameraTexture;
            lock (lockObject) {
                byte[] rgbBytes = NV21Converter.ToRGB(imageBuffer, imageWidth, imageHeight);

                Debug.Log("PoseImageCreator: ARGBBytes created! Byte array length = " + rgbBytes.Length);

                cameraTexture = new Texture2D(imageWidth, imageHeight, TextureFormat.ARGB32, false);
                cameraTexture.LoadRawTextureData(rgbBytes);
                cameraTexture.Apply();
            }

            return cameraTexture;
#else
            // Assumes Editor mode

            Texture2D cameraTexture = null;
            lock (lockObject)
            {
                //// Get a render texture for the main camera
                //RenderTexture rtTemp = new RenderTexture(renderCamera.pixelWidth, renderCamera.pixelHeight, 0, RenderTextureFormat.ARGB32);
                //Camera.main.targetTexture = rtTemp;
                RenderTexture rt = editorRenderCamera.targetTexture;
                rt.width = editorRenderCamera.pixelWidth;
                rt.height = editorRenderCamera.pixelHeight;

                // Generate a texture from the render texture
                cameraTexture = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
                Graphics.CopyTexture(rt, cameraTexture);
                cameraTexture.Apply();

                //// Undo the target texture
                //Camera.main.targetTexture = null;
                //GameObject.DestroyImmediate(rt);
            }

            return cameraTexture;
#endif
        }

        /// <summary>
        /// Doesn't work after testing...OpenGL Invalid Enum error - can't track down. Supposedly is popping up in ARCore as well. Find workaround!
        /// </summary>
        /// <returns></returns>
        public int getARTexturePointer()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log("PoseImageCreator: Attempting to get AR Texture handle now...");

            uint tex = PublicTangoAPI.TangoUnity_getArTexture();

            Debug.Log("PoseImageCreator: Handle = " + tex);

            return (int)tex;
#else
            return 0;
#endif
        }
#endregion
#endregion
    }
}
#endif