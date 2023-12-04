using UnityEngine;
using VRUEAssignments.Utils;

namespace VRUEAssignments.XR
{
    public class XRRigReferences : UnitySingleton<XRRigReferences>
    {
        public Transform Root;
        public Transform Head;
        public Transform LeftHand;
        public Transform RightHand;
    }
}
