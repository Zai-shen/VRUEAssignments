using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

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
        {
            return;
        }

        UpdatePositions();
        UpdateRotations();
    }

    private void UpdateRotations()
    {
        Root.position = XRRigReferences.Instance.Root.position;
        Head.position = XRRigReferences.Instance.Head.position;
        LeftHand.position = XRRigReferences.Instance.LeftHand.position;
        RightHand.position = XRRigReferences.Instance.RightHand.position;
    }

    private void UpdatePositions()
    {
        Root.rotation = XRRigReferences.Instance.Root.rotation;
        Head.rotation = XRRigReferences.Instance.Head.rotation;
        LeftHand.rotation = XRRigReferences.Instance.LeftHand.rotation;
        RightHand.rotation = XRRigReferences.Instance.RightHand.rotation;    }
}
