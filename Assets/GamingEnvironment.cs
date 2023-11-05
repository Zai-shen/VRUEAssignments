using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamingEnvironment : MonoBehaviour
{
    public GameObject PoolBallSetPrefab;
    public GameObject PoolSpace;
    
    public float Scale = 1f;


    void Start()
    {
        
    }


    [ContextMenu("Restart")]
    public void Restart()
    {
        ResetBallPositions();
    }

    private void ResetBallPositions()
    {
        Destroy(PoolSpace.transform.GetChild(0).gameObject);
        Instantiate(PoolBallSetPrefab, PoolSpace.transform);
    }
}
