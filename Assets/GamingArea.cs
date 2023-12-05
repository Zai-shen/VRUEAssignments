using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamingArea : MonoBehaviour
{
    private Grid _gamingAreaGrid;

    private void Start()
    {
        _gamingAreaGrid = new Grid(10, 10, 1, transform.position);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            _gamingAreaGrid.SetValue(worldPos,42);
        }
    }
}
