using UnityEngine;

namespace VRUEAssignments.Utils
{
    public struct Destination
    {
        public Destination(GameObject go, Vector3 position, Quaternion rotation)
        {
            Go = go;
            Position = position;
            Rotation = rotation;
        }
        
        public Destination(GameObject go, Vector3 position, Vector3 eulerRotation)
        {
            Go = go;
            Position = position;
            Rotation = Quaternion.Euler(eulerRotation);
        }
        
        public GameObject Go;
        public Vector3 Position;
        public Quaternion Rotation;
    }
}