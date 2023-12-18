using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRUEAssignments.UI;

public class TileBase : MonoBehaviour
{
    [SerializeField] private int _maxHealthPoints = 40;
    [SerializeField] private int _currenthealthPoints;
    [SerializeField] private HealthBar _healthBar;

    private void Start()
    {
        _currenthealthPoints = _maxHealthPoints;
        _healthBar.UpdateHealthBar(_maxHealthPoints, _currenthealthPoints);
    }

    public void TakeDamage(int dmg)
    {
        _currenthealthPoints -= dmg;
        if (_currenthealthPoints <= 0)
        {
            Debug.LogWarning("You lost!");
            Destroy(gameObject);
        }
        else
        {
            _healthBar.UpdateHealthBar(_maxHealthPoints, _currenthealthPoints);
        }
    }
}
