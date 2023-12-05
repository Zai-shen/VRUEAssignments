using System.Collections;
using UnityEngine;

namespace VRUEAssignments.NPCs
{
    public class EnemyBehaviour : MonoBehaviour
    {
        private Material objMaterial;
        public Material dieMaterial;
        private MeshRenderer objRenderer;
        // Start is called before the first frame update
        void Start()
        {
            objRenderer = this.GetComponent<MeshRenderer>();
            objMaterial =  objRenderer.material;
        }


        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            // Debug.Log("Enemy entered collision with " + other.gameObject.name);

            if (other.gameObject.name.Equals("ClearArea")) {
                Destroy(this.gameObject);

            }

        
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag.Equals("Bullet"))
            {
                StartCoroutine(blinkAndDestroy());
            }
        }

        IEnumerator blinkAndDestroy()
        {
            objRenderer.material = dieMaterial;
            yield return new WaitForSeconds(0.3f);
            objRenderer.material = objMaterial;
            yield return new WaitForSeconds(0.3f);
            objRenderer.material = dieMaterial;
            yield return new WaitForSeconds(0.3f);
            objRenderer.material = objMaterial;
            yield return new WaitForSeconds(0.3f);
            Destroy(this.gameObject);
        }
    }
}
