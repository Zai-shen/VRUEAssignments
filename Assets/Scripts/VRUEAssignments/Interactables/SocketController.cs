using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketRotation : MonoBehaviour
{
    private XRSocketInteractor socketInteractor;

    [SerializeField] GameObject anchor;
    // Start is called before the first frame update
    void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();

    
    }

    // Update is called once per frame
    void Update()
    {
        if (socketInteractor.isHoverActive) {
            var allHoveredInteractables = socketInteractor.interactablesHovered;
            if (allHoveredInteractables.Count == 1)
            {
                var rotation = allHoveredInteractables[0].transform.eulerAngles;
                anchor.transform.eulerAngles = new Vector3(0, rotation.y, 0);
            }
        }
    }

}
