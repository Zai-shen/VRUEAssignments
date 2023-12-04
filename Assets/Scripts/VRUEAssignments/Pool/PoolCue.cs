using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VRUEAssignments.Managers;

namespace VRUEAssignments.Pool
{
    public class PoolCue : MonoBehaviour
    {
        public ActionBasedController LeftController;
        public ActionBasedController RightController;
        public float CoolDown = 0.5f;
        private List<string> _ballsOnCooldown = new();
    
        private void OnCollisionEnter(Collision collision)
        {
            Transform collisionT = collision.transform;
        
            if (collisionT.CompareTag("PoolBall") && !IsOnCooldown(collisionT.name))
            {
                GameStatistics.CueHits++;
                UIManager.Instance.UpdateCueHits();

                HapticFeedback();
            
                StartCoroutine(HandleCooldown(collisionT.name));
            }
        }

        private void HapticFeedback()
        {
            LeftController?.SendHapticImpulse(0.125f, 0.175f);
            RightController?.SendHapticImpulse(0.125f, 0.175f);
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
}
