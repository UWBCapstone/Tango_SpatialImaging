using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.InteropServices; // DllImport

#if UNITY_EDITOR || UNITY_ANDROID
using Tango;

namespace CloakingBox { 
    public struct PublicTangoAPI
        {
#if UNITY_ANDROID && !UNITY_EDITOR
                [DllImport(Common.TANGO_CLIENT_API_DLL)]
                public static extern int TangoService_updateTexture(
                    TangoEnums.TangoCameraId cameraId, ref double timestamp);

                [DllImport(Common.TANGO_CLIENT_API_DLL)]
                public static extern int TangoService_updateTextureExternalOes(
                    TangoEnums.TangoCameraId cameraId, UInt32 glTextureId, out double timestamp);

                [DllImport(Common.TANGO_CLIENT_API_DLL)]
                public static extern int TangoService_getCameraIntrinsics(
                    TangoEnums.TangoCameraId cameraId, [Out] TangoCameraIntrinsics intrinsics);

                [DllImport(Common.TANGO_CLIENT_API_DLL)]
                public static extern int TangoService_connectOnImageAvailable(
                    TangoEnums.TangoCameraId cameraId, IntPtr context,
                    [In, Out] APIOnImageAvailable callback);

                [DllImport(Common.TANGO_CLIENT_API_DLL)]
                public static extern int TangoService_connectOnTextureAvailable(
                    TangoEnums.TangoCameraId cameraId, IntPtr ContextMenu, APIOnTextureAvailable callback);

                [DllImport(Common.TANGO_CLIENT_API_DLL)]
                public static extern int TangoService_Experimental_connectTextureIdUnity(
                    TangoEnums.TangoCameraId id, UInt32 texture_y, UInt32 texture_Cb, UInt32 texture_Cr, IntPtr context,
                    APIOnTextureAvailable callback);

                [DllImport(Common.TANGO_UNITY_DLL)]
                public static extern UInt32 TangoUnity_getArTexture();

                [DllImport(Common.TANGO_UNITY_DLL)]
                public static extern void TangoUnity_setRenderTextureUVs(Vector2[] uv);

                [DllImport(Common.TANGO_UNITY_DLL)]
                public static extern void TangoUnity_setRenderTextureDistortion(TangoCameraIntrinsics intrinsics);

                [DllImport(Common.TANGO_UNITY_DLL)]
                public static extern IntPtr TangoUnity_getRenderTextureFunction();
#endif


        /// <summary>
        /// Tango video overlay C callback function signature.
        /// </summary>
        /// <param name="context">Callback context.</param>
        /// <param name="cameraId">Camera ID.</param>
        /// <param name="image">Image buffer.</param>
        /// <param name="cameraMetadata">Camera metadata.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void APIOnImageAvailable(
            IntPtr context, TangoEnums.TangoCameraId cameraId, ref TangoImage image,
            ref TangoCameraMetadata cameraMetadata);

        /// <summary>
        /// Tango camera texture C callback function signature.
        /// </summary>
        /// <param name="context">Callback context.</param>
        /// <param name="cameraId">Camera ID.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void APIOnTextureAvailable(IntPtr context, TangoEnums.TangoCameraId cameraId);
    }
}
#endif