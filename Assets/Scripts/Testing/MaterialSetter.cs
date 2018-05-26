using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class MaterialSetter : MonoBehaviour
    {
        public GameObject go;
        public PoseImageCreator imageCreator;

        public void Start()
        {
            //if(go != null)
            //{
            //    var mr = go.GetComponent<MeshRenderer>();
            //    if (mr != null)
            //    {
            //        mr.material = new Material(Shader.Find("Custom/ImageSetter"));
            //    }
            //}
        }

        public void SetTextureToCameraImage()
        {
            var tex = imageCreator.MakeCameraImage();
            if(tex != null)
            {
                SetTexture(tex);
            }
        }

        public void SetTexture(Texture2D tex)
        {
            var mat = getMaterial();
            mat.SetTexture("_MainTex", tex);
        }

        private Material getMaterial()
        {
            return go.GetComponent<MeshRenderer>().material;
        }
    }
}