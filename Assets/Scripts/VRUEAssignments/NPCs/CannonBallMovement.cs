using Unity.VisualScripting;
using UnityEngine;

namespace VRUEAssignments.NPCs
{
    public class CannonBallMovement : MonoBehaviour
    {
   
        public float speed;
        public float shootingRange;
        private Vector3 initPosition;
        private Vector3 currentPosition;
        // Start is called before the first frame update
        void Start()
        {
            initPosition = transform.position;   
            currentPosition = transform.position;   
        }

        // Update is called once per frame
        void Update()
        {
            if (!this.gameObject.IsDestroyed() && Vector3.Distance(initPosition, currentPosition) < shootingRange)
            {
                transform.localPosition += Vector3.up * Time.deltaTime * speed;
                currentPosition = transform.position;
            } else if (!this.gameObject.IsDestroyed())
            {
                Destroy(this.gameObject);
            }
        
        }

        private void OnTriggerEnter(Collider other)
        {
            Destroy(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.tag.Equals("Tower"))
            {
                Destroy(this.gameObject);

            }
        }
    }
}
