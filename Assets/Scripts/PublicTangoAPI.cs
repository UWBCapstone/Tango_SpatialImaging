using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

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
    }
}
#endif