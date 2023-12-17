using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRUEAssignments.Map;

public class MapNotifyingTrigger : MonoBehaviour
{
    public Vector3 WorldPos;

    private void Start()
    {
        WorldPos = transform.position;
    }

    public void NotifyMap()
    {
        Map.Instance.PlaceMapTile(WorldPos);
    }
}
