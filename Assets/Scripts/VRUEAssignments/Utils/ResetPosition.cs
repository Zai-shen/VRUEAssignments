using UnityEngine;
using VRUEAssignments.Networking;

namespace VRUEAssignments.Utils
{
    public class ResetPosition : MonoBehaviour
    {
        public Transform XROrigin;

        public NetworkManager ANetworkManager;

        public void DoResetPosition()
        {
            XROrigin.transform.position = ANetworkManager._currentSpawnOffset;
        }
    }
}
