using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class DeleteObjectOnRelease: MonoBehaviour
{
    XRGrabInteractable m_GrabInteractable;

    [SerializeField] float resetDelayTime;
    protected bool shouldDestroy { get; set; }
    bool isController;
    private bool isGrabbing;

    private Quaternion initRotation;

    // Start is called before the first frame update
    void Awake()
    {
        m_GrabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        m_GrabInteractable.selectExited.AddListener(OnSelectExit);
        m_GrabInteractable.selectEntered.AddListener(OnSelect);
    }

    private void OnDisable()
    {
        m_GrabInteractable.selectExited.RemoveListener(OnSelectExit);
        m_GrabInteractable.selectEntered.RemoveListener(OnSelect);
    }

    private void OnSelect(SelectEnterEventArgs arg0) {
      
        CancelInvoke("DoDestroy");

        isGrabbing = true;

        
    }
    private void OnSelectExit(SelectExitEventArgs arg0) 
    {
        if (shouldDestroy)
        {
            Invoke(nameof(DoDestroy), resetDelayTime);

        }
        isGrabbing = false;
    }

    protected virtual void DoDestroy()
    {

        if (shouldDestroy)
        {
            Destroy(this.gameObject);
        }
           
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (ControllerCheck(other.gameObject))
            return;
        var socketInteractor = other.gameObject.GetComponent<XRSocketInteractor>();

        if (socketInteractor == null)
        {
            shouldDestroy = true;
           // Debug.Log("Returning home bc no socket interactor found");
            return;
        }

        var interactableHovered = socketInteractor.GetOldestInteractableHovered();
        if (interactableHovered != null && 
            interactableHovered.transform.gameObject.name.Equals(this.gameObject.name) && 
            socketInteractor.CanHover(interactableHovered))
        {
            shouldDestroy = false;
          //  Debug.Log("Not returning home, bc socket active");
        }
        else
        {
            shouldDestroy = true; //The socket interactor exists and CANNOT select the Grab object
           // Debug.Log("Returning home because socket is blocked");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ControllerCheck(other.gameObject))
            return;

        shouldDestroy = true;
      //  Debug.Log("Returning home because no socket is active");
    }

    bool ControllerCheck(GameObject collidedObject)
    {
        //first check that this is not the collider of a controller
        isController = collidedObject.gameObject.GetComponent<XRBaseController>() != null ? true : false;
        return isController;
    }
}


