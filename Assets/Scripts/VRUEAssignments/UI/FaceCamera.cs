using System;
using UnityEngine;

namespace VRUEAssignments.UI
{
    public class FaceCamera : MonoBehaviour
    {
        private Camera mainCam;

        private void Start()
        {
            mainCam = Camera.main;
        }

        private void LateUpdate()
        {
            if (!mainCam) return;
            
            transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        }
    }
}