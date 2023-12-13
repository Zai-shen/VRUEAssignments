using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class ResetObject : MonoBehaviour
{
    XRGrabInteractable m_GrabInteractable;

    [Tooltip("The Transform that the object will return to")]
    [SerializeField] Vector3 returnToPosition;
    [Tooltip("Specify a movable object instead of position that the object returns to")]
    [SerializeField] GameObject returnToAnchor;
    [SerializeField] float homeBaseScale;
    [SerializeField] float resetDelayTime;
    protected bool shouldReturnHome { get; set; }
    bool isController;
    private bool isGrabbing;

    private Quaternion initRotation;

    // Start is called before the first frame update
    void Awake()
    {
        m_GrabInteractable = GetComponent<XRGrabInteractable>();
        returnToPosition = this.transform.position;
        initRotation = this.transform.rotation;
        shouldReturnHome = true;
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
      
        CancelInvoke("ReturnHome");

        isGrabbing = true;

        
    }
    private void OnSelectExit(SelectExitEventArgs arg0) 
    {
        if (shouldReturnHome)
        {
            Invoke(nameof(ReturnHome), resetDelayTime);

        }
        isGrabbing = false;
    }

    protected virtual void ReturnHome()
    {

        if (shouldReturnHome)
        {
            transform.position = returnToAnchor.transform.position;
            transform.rotation = returnToAnchor.transform.rotation;
            transform.localScale = new Vector3(homeBaseScale, homeBaseScale, homeBaseScale);
        }
           
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (ControllerCheck(other.gameObject))
            return;
        var socketInteractor = other.gameObject.GetComponent<XRSocketInteractor>();

        if (socketInteractor == null)
        {
            shouldReturnHome = true;
           // Debug.Log("Returning home bc no socket interactor found");
            return;
        }

        var interactableHovered = socketInteractor.GetOldestInteractableHovered();
        if (interactableHovered != null && 
            interactableHovered.transform.gameObject.name.Equals(this.gameObject.name) && 
            socketInteractor.CanHover(interactableHovered))
        {
            shouldReturnHome = false;
          //  Debug.Log("Not returning home, bc socket active");
        }
        else
        {
            shouldReturnHome = true; //The socket interactor exists and CANNOT select the Grab object
           // Debug.Log("Returning home because socket is blocked");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ControllerCheck(other.gameObject))
            return;

        shouldReturnHome = true;
      //  Debug.Log("Returning home because no socket is active");
    }

    bool ControllerCheck(GameObject collidedObject)
    {
        //first check that this is not the collider of a controller
        isController = collidedObject.gameObject.GetComponent<XRBaseController>() != null ? true : false;
        return isController;
    }
}


