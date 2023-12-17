using Dreamteck.Splines;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace VRUEAssignments.NPCs
{
    public class AddPathToEnemy : MonoBehaviour
    {
        public GameObject enemyContainer;
        public SplineComputer spline;

        void Start()
        {
            foreach (Transform child in enemyContainer.transform)
            {
                child.GetComponent<SplineFollower>().spline = spline;
            }
            
        }
    }
}
