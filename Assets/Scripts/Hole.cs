using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public float expulsionForce = 3f;
    
    private SplineComputer _splineComputer;
    private GameObject _currentCollisionGO;
    
    private void Start()
    {
        _splineComputer = GetComponent<SplineComputer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PoolBall"))
        {
            _currentCollisionGO = collision.gameObject;
            
            Rigidbody rb = _currentCollisionGO.GetComponent<Rigidbody>();
            rb.isKinematic = true;

            SplinePoint sp = _splineComputer.GetPoint(0);
            sp.position = _currentCollisionGO.transform.position;
            
            SplineFollower splineFollower = _currentCollisionGO.AddComponent<SplineFollower>();
            splineFollower.spline = _splineComputer;
            splineFollower.onEndReached += SetBallOutside;
        }
    }

    private void SetBallOutside(double d)
    {
        Rigidbody rb = _currentCollisionGO.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        Vector3 direction = (_currentCollisionGO.transform.position - transform.position).normalized;
        rb.AddForce(direction * expulsionForce, ForceMode.Impulse);

        SplineFollower splineFollower = _currentCollisionGO.GetComponent<SplineFollower>();
        splineFollower.enabled = false;
    }
}
