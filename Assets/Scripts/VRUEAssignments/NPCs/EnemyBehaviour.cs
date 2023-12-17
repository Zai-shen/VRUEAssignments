using System.Collections;
using UnityEngine;

namespace VRUEAssignments.NPCs
{
    public class EnemyBehaviour : MonoBehaviour
    {
        private Material objMaterial;
        public Material dieMaterial;
        private MeshRenderer objRenderer;
        private Animator mAnimator;
        public int lives;
        // Start is called before the first frame update
        void Start()
        {
            //objRenderer = this.GetComponent<MeshRenderer>();
            //objMaterial =  objRenderer.material;
            mAnimator = GetComponent<Animator>();
        }


        // Update is called once per frame
        void Update()
        {
        
        }
        
        private void OnTriggerEnter(Collider other)
        {
            

            if (other.gameObject.name.Equals("ClearArea")) {
                Destroy(this.gameObject);

            }

            if (other.gameObject.tag.Equals("Bullet"))
            {
                StartCoroutine(blinkAndDestroy());
            }


        }

        public void OnHitByParticle(int damage)
        {
            Debug.Log("Object" + this.gameObject.name + " got hit with damage " + damage + ". Current lives: " + lives);
            lives -= damage;
            if (lives <= 0) {
                mAnimator.SetTrigger("getShot");
                StartCoroutine(blinkAndDestroy());
            }
            else
            {
                mAnimator.SetTrigger("getHit");
            }
            
           
        }

        IEnumerator blinkAndDestroy()
        {
            var length = mAnimator.GetCurrentAnimatorClipInfo(0).Length;
            yield return new WaitForSeconds(length);
            Destroy(this.gameObject);
        }
    }
}
