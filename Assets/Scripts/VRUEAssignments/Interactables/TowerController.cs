
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using VRUEAssignments.NPCs;

public class TowerController : MonoBehaviour
{
    [SerializeField] GameObject particleGameObject;
    [SerializeField] GameObject cannonBallSpawnPoint;
    [SerializeField] float inventoryScale;
    [SerializeField] float worldScale;
    [SerializeField] int destroyForce;

    private Ray ray;
    private bool isCurrentlyShooting;
    private GameObject lastHitGameObject;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = new Vector3(inventoryScale, inventoryScale, inventoryScale);
        particleGameObject.SetActive(false);
        ray = new Ray(cannonBallSpawnPoint.transform.position, cannonBallSpawnPoint.transform.forward);


    }
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(ray.origin, ray.direction *10, Color.green);
        if (isCurrentlyShooting)
        {
            ray.origin = cannonBallSpawnPoint.transform.position;
            ray.direction = cannonBallSpawnPoint.transform.forward;
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData))
            {
                var hitGameObject = hitData.transform.gameObject;
                // only hit a game object once!
                if (lastHitGameObject != null && lastHitGameObject.Equals(hitGameObject))
                {
                    return;
                }
                EnemyBehaviour enemyBehaviour;
                if (hitGameObject.TryGetComponent(out enemyBehaviour)) {
                    enemyBehaviour.OnHitByParticle(destroyForce);
                    lastHitGameObject = hitGameObject;
                }
            }
        }
    }

    public void ToggleCannonBallMovement(bool enable)
    {
        isCurrentlyShooting = enable;
        StartCoroutine(WaitAndControlParticles(enable));
        
      
    }

    private IEnumerator WaitAndControlParticles(bool enable)
    {
        yield return new WaitForSeconds(0.5f);
        particleGameObject.SetActive(enable);
       
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered collisiton with " + other.gameObject.name);
        if (other.gameObject.name.Equals("Inventory"))
        {
          
            Debug.Log("Scaling to inventory scale (small)");
            this.transform.localScale = new Vector3(inventoryScale, inventoryScale, inventoryScale);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited collisiton with " + other.gameObject.name);
        if (other.gameObject.name.Equals("Inventory"))
        {
           
            Debug.Log("Scaling to world scale (large)");
            this.transform.localScale = new Vector3(worldScale, worldScale, worldScale);
        }
    
    }

}
