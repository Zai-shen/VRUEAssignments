
using System;
using UnityEngine;
using UnityEngine.Rendering;
using VRUEAssignments.NPCs;

public class TowerController : MonoBehaviour
{
    private ContinuousObjectSpawner objectSpawnerComponent;
    [SerializeField] GameObject cannonBallSpawnPoint;
    [SerializeField] float inventoryScale;
    [SerializeField] float worldScale;
    private bool objectInInventoryArea;


    // Start is called before the first frame update
    void Start()
    {
        objectSpawnerComponent = cannonBallSpawnPoint.GetComponent<ContinuousObjectSpawner>();
        objectSpawnerComponent.enabled = false;
        this.transform.localScale = new Vector3(inventoryScale, inventoryScale, inventoryScale);   
        

    }
    // Update is called once per frame
    void Update()
    {
       
    }

    public void ToggleCannonBallMovement(bool enable)
    {
        objectSpawnerComponent.enabled = enable;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered collisiton with " + other.gameObject.name);
        if (other.gameObject.name.Equals("Inventory"))
        {
            objectInInventoryArea = true;
            Debug.Log("Scaling to inventory scale (small)");
            this.transform.localScale = new Vector3(inventoryScale, inventoryScale, inventoryScale);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited collisiton with " + other.gameObject.name);
        if (other.gameObject.name.Equals("Inventory"))
        {
            objectInInventoryArea = false;
            Debug.Log("Scaling to world scale (large)");
            this.transform.localScale = new Vector3(worldScale, worldScale, worldScale);
        }
    
    }

}
