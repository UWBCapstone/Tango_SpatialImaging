using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class Testing_TouchScreenManager : MonoBehaviour
    {
        public MaterialSetter ms;

        public void Update()
        {
            if(Input.touchCount > 0) { 
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Began)
                {
                    OnTap();
                }
            }
        }

        public void OnTap()
        {
            Debug.Log("Testing_TouchScreenManager: Tap received!");
            ms.SetTextureToCameraImage();
        }
    }
}