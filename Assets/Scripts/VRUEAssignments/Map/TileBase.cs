using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    [SerializeField] private int _healthPoints = 40;
    
    public void TakeDamage(int dmg)
    {
        _healthPoints -= dmg;
        if (_healthPoints <= 0)
        {
            Debug.LogWarning("You lost!");
        }
    }
}
