using Dreamteck.Splines;
using UnityEngine;

namespace VRUEAssignments.NPCs
{
    public class AddPathToEnemy : MonoBehaviour
    {
        public GameObject enemy;
        public SplineComputer spline;
        void Start()
        {
            enemy.GetComponent<SplineFollower>().spline = spline;
        }
    }
}
