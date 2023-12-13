using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VRUEAssignments.NPCs;

public class SocketController : MonoBehaviour
{
    private XRSocketInteractor socketInteractor;

    [SerializeField] GameObject anchor;
    // Start is called before the first frame update

    private GameObject selectedGameObject;
    void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();

    
    }

    // Update is called once per frame
    void Update()
    {
        if (socketInteractor.isHoverActive) {
            var hoveredInteractable = socketInteractor.GetOldestInteractableHovered();
            if (hoveredInteractable != null)
            {
                var rotation = hoveredInteractable.transform.eulerAngles;
                anchor.transform.eulerAngles = new Vector3(0, rotation.y, 0);
            }
        }
    }

    public void OnSelectEntered()
    {
        Debug.Log("Some object entered the socket");
        var allSelectedInteractables = socketInteractor.interactablesSelected;
        if (allSelectedInteractables.Count == 1)
        {
            selectedGameObject = allSelectedInteractables[0].transform.gameObject;
        }
        ToggleCannonBallShooting(true);
    }

    

    public void OnSelectExited()
    {
        Debug.Log("Some object exited the socket");
        ToggleCannonBallShooting(false);

        var cannonBalls = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (var cannonBall in cannonBalls)
        {
            Destroy(cannonBall.gameObject);
        }
    }

    private void ToggleCannonBallShooting(bool isEnabled)
    {
       
        if (selectedGameObject == null)
        {
            Debug.Log("Error last interacted object is null");
            return;
        }
        var towerController = selectedGameObject.GetComponent<TowerController>();
        Debug.Log("Interactable in socket: " + selectedGameObject.transform.gameObject.name);
        if (towerController != null)
        {
            towerController.ToggleCannonBallMovement(isEnabled);
        }
        
    }
}
