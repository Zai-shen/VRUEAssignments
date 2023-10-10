using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SizeToMass : MonoBehaviour
{
    private Rigidbody _rb;
    private float _startingMass;
    private Vector3 _startingSize;
    private Vector3 _currentSize;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _startingMass = _rb.mass;
        _startingSize = _currentSize = transform.localScale;
    }

    private void Update()
    {
        Vector3 cSize = transform.localScale;
        
        if (Vector3.Distance(cSize, _currentSize) > 0.01f)
        {
            //Just multiply by [max float in vector], u should've defined it clearer...
            _rb.mass = _startingMass * transform.localScale.y;

            _currentSize = cSize;
        }
        
    }
}
