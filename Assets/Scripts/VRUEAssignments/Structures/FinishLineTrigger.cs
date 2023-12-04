using UnityEngine;

namespace VRUEAssignments.Structures
{
    public class FinishLineTrigger : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
        }
    }
}
