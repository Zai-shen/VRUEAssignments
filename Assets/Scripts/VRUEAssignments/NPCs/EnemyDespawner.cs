using UnityEngine;

namespace VRUEAssignments.NPCs
{
    public class DestroyEnemies : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Entered collision with " + other.gameObject.name);
            //Destroy(other.gameObject);
        }
    }
}
