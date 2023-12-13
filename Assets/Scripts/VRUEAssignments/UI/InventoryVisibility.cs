using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryVisibility : MonoBehaviour
{

    public InputAction toggleVisibilityAction;
    private bool isVisible = false;
    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
        toggleVisibilityAction.Enable();
        toggleVisibilityAction.performed += context => onToggleVisibility(context);
    }

    private void onToggleVisibility(InputAction.CallbackContext context)
    {
        Debug.Log("Toggle visibility performed!");
        isVisible = !isVisible;
        Debug.Log("Inventory visible: " + isVisible);
        this.gameObject.SetActive(isVisible);
    }
   

    private void OnEnable()
    {
        toggleVisibilityAction.Enable();
    }

    private void OnDisable()
    {
        Debug.Log("Disable called!");
        toggleVisibilityAction.Disable();
    }
}
