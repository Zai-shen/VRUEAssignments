using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPathToEnemy : MonoBehaviour
{
    public GameObject enemy;
    public SplineComputer spline;
    void Start()
    {
        enemy.GetComponent<SplineFollower>().spline = spline;
    }
}
