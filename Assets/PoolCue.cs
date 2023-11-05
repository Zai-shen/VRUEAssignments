using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRUEAssignments.Managers;

public class PoolCue : MonoBehaviour
{
    public float CoolDown = 0.5f;
    private List<string> _ballsOnCooldown = new();
    
    private void OnCollisionEnter(Collision collision)
    {
        Transform collisionT = collision.transform;
        
        if (collisionT.CompareTag("PoolBall") && !IsOnCooldown(collisionT.name))
        {
            GameStatistics.CueHits++;
            UIManager.Instance.UpdateCueHits();
            
            StartCoroutine(HandleCooldown(collisionT.name));
        }
    }

    private IEnumerator HandleCooldown(string name)
    {
        _ballsOnCooldown.Add(name);
        yield return new WaitForSeconds(CoolDown);
        _ballsOnCooldown.Remove(name);
        yield return null;
    }

    private bool IsOnCooldown(string name)
    {
        return _ballsOnCooldown.Contains(name);
    }
}
