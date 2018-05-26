using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public enum TestingScenarioTypes
    {
        DebuggingImageToPlane,
        DebuggingPoseImageDictionaryUpdate,
        DebuggingPoseVoxelSet
    }

    public class Testing_TouchScreenManager : MonoBehaviour
    {
        public TestingScenarioTypes scenario;
        public MaterialSetter ms;
        public PoseImageDictionary poseImageMap;
        public PoseVoxelSet poseVoxelSet;

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
            switch (scenario)
            {
                case TestingScenarioTypes.DebuggingImageToPlane:
                    onTap_DebuggingImageToPlane();
                    break;
                case TestingScenarioTypes.DebuggingPoseImageDictionaryUpdate:
                    onTap_DebuggingPoseImageDictionaryUpdate();
                    break;
                case TestingScenarioTypes.DebuggingPoseVoxelSet:
                    onTap_DebuggingPoseVoxelSet();
                    break;
            }
        }

        private void onTap_DebuggingImageToPlane()
        {
            Debug.Log("Testing_TouchScreenManager: Tap received! Testing Image to Plane for image capture from Tango!");
            ms.SetTextureToCameraImage();
        }

        private void onTap_DebuggingPoseImageDictionaryUpdate()
        {
            Debug.Log("Testing_TouchScreenManager: Tap received! Testing Pose Image Dictionary update!");
            var poseImageDictionaryTestScript = GameObject.FindObjectOfType<Testing_PoseImageDictionary>();
            PoseVoxel voxel = poseVoxelSet.ToPoseVoxel(new Pose(Camera.main));
            Texture2D tex = Texture2D.whiteTexture;

            poseImageDictionaryTestScript.Add(voxel, tex);
        }

        private void onTap_DebuggingPoseVoxelSet()
        {
            Debug.Log("Testing_TouchScreenManager: Tap received! Testing Pose Voxel Set!");
            var poseVoxelSetTestScript = GameObject.FindObjectOfType<Testing_PoseVoxelSet>();
            if (!poseVoxelSetTestScript.AddedIsSameAsStored(poseVoxelSetTestScript.GetCurrentPose()))
            {
                Debug.LogError("Testing_TouchScreenManager: Added pose + image is not the same as what is stored!");
            }
            else
            {
                Debug.Log("Testing_TouchScreenManager: Added pose + image is the same as what is stored.");
            }
        }
    }
}