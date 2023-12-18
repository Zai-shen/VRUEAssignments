using Photon.Pun;
using UnityEngine;

namespace VRUEAssignments.XR
{
    public class Character : MonoBehaviourPun
    {
        public Transform Root;
        public Transform Head;
        public Transform LeftHand;
        public Transform RightHand;

        public Renderer[] meshesToHide;

        private void Start()
        {
            if (photonView.IsMine && PhotonNetwork.IsConnected == true)
            {
                foreach (Renderer rend in meshesToHide)
                {
                    rend.enabled = false;
                }
            }
        }

        private void Update()
        {
            // Prevent control is connected to Photon and represent the localPlayer
            if( photonView.IsMine == false && PhotonNetwork.IsConnected == true )
                return;

            // Check for XRRigReferences
            if (XRRigReferences.Instance == null)
                return;

            UpdatePositions();
            UpdateRotations();
        }

        private void UpdatePositions()
        {
            Root.position = XRRigReferences.Instance.Root.position;
            Head.position = XRRigReferences.Instance.Head.position;
            LeftHand.position = XRRigReferences.Instance.LeftHand.position;
            RightHand.position = XRRigReferences.Instance.RightHand.position;
        }

        private void UpdateRotations()
        {
            Root.rotation = XRRigReferences.Instance.Root.rotation;
            Head.rotation = XRRigReferences.Instance.Head.rotation;
            LeftHand.rotation = XRRigReferences.Instance.LeftHand.rotation;
            RightHand.rotation = XRRigReferences.Instance.RightHand.rotation;    }
    }
}
