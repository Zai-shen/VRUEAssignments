using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousObjectSpawner: MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obj;
    private float time;
    public float spawnInterval;
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= spawnInterval)
        {
            var newObject = Instantiate(obj, transform.position, Quaternion.identity);
            newObject.transform.parent = transform;
            time = 0;
        }
    }
}
