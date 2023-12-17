using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] GameObject tower1;
    [SerializeField] GameObject tower2;
    [SerializeField] GameObject tower3;
    [SerializeField] GameObject anchorTower1;
    [SerializeField] GameObject anchorTower2;
    [SerializeField] GameObject anchorTower3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTower1SpawnButtonPressed()
    {
        SpawnTower(tower1, anchorTower1);
    }

    public void OnTower2SpawnButtonPressed()
    {
        SpawnTower(tower2, anchorTower2);
    }

    public void OnTower3SpawnButtonPressed()
    {
        SpawnTower(tower3, anchorTower3);
    }
    private void SpawnTower(GameObject tower, GameObject placeHolder)
    {
        if (placeHolder.transform.childCount == 0)
        {
            var newObject = Instantiate(tower, placeHolder.transform.position, placeHolder.transform.rotation);
            newObject.transform.SetParent(placeHolder.transform);
        }
        //Debug.Log("Not spawning, bc place is already used!");
        
    }
}
