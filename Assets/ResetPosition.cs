using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    public Transform XROrigin;

    public NetworkManager ANetworkManager;

    [ContextMenu("DoResetPosition")]
    public void DoResetPosition()
    {
        XROrigin.transform.position = ANetworkManager._currentSpawnOffset;
    }
}
