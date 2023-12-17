using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryVisibility : MonoBehaviour
{

    public InputAction toggleVisibilityAction;
    private bool isVisible = false;
    public GameObject inventoryHint;
    // Start is called before the first frame update
    void Start()
    {
        this.enabled = false;
        toggleVisibilityAction.Enable();
        toggleVisibilityAction.performed += context => onToggleVisibility(context);
    }

    private void onToggleVisibility(InputAction.CallbackContext context)
    {
       
        isVisible = !isVisible;
        this.gameObject.SetActive(isVisible);
        inventoryHint.SetActive(!isVisible);
    }
    
    public void OnCloseInventory()
    {
        isVisible = false;
        this.gameObject.SetActive(isVisible);
        inventoryHint.SetActive(true);
    }

    public void OnOpenInventory()
    {
        isVisible = true;
        this.gameObject.SetActive(isVisible);
        inventoryHint.SetActive(false);
    }

    private void OnEnable()
    {
        toggleVisibilityAction.Enable();
    }

    private void OnDisable()
    {
        //Debug.Log("Disable called!");
        toggleVisibilityAction.Disable();
    }
}
