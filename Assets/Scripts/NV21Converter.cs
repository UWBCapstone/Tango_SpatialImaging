// This work is borrowed from Cheok Yan Cheng and referenced from
// https://stackoverflow.com/questions/12469730/confusion-on-yuv-nv21-conversion-to-rgb?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class NV21Converter : MonoBehaviour
    {
        public static byte[] ToRGB(byte[] yuv, int width, int height)
        {
            List<byte> rgbList = new List<byte>();
            
            int frameSize = width * height;
            const int ii = 0;
            const int ij = 0;
            const int di = 1; // +1 (byte operator 1 << 0?)
            const int dj = 1; // +1 (byte operator 1 << 0?)

            int a = 0;
            for(int i = 0, ci = ii; i < height; ++i, ci += di)
            {
                for(int j = 0, cj = ij; j < width; ++j, cj += dj)
                {
                    int y = (0xff & ((int)yuv[ci * width + cj]));
                    int v = (0xff & ((int)yuv[frameSize + (ci >> 1) * width + (cj & ~1) + 0]));
                    int u = (0xff & ((int)yuv[frameSize + (ci >> 1) * width + (cj & ~1) + 1]));
                    y = y < 16 ? 16 : y;

                    int r = (int)(1.164f * (y - 16) + 1.596f * (v - 128));
                    int g = (int)(1.164f * (y - 16) - 0.831f * (v - 128) - 0.391f * (u - 128));
                    int b = (int)(1.164f * (y - 16) + 2.018f * (u - 128));

                    r = r < 0 ? 0 : (r > 255 ? 255 : r);
                    g = g < 0 ? 0 : (g > 255 ? 255 : g);
                    b = b < 0 ? 0 : (b > 255 ? 255 : b);

                    //rgbList.Add((byte)(0xff000000 | (r << 16) | (g << 8) | b));

                    rgbList.Add(255);
                    rgbList.Add((byte)r);
                    rgbList.Add((byte)g);
                    rgbList.Add((byte)b);
                }
            }

            return rgbList.ToArray();
        } 
    }
}