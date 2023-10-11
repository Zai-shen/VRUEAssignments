using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Template.VR.VRUEAssignments.Structures
{
    public class ExplodingStructure : Structure
    {
        [SerializeField] private float ExplosionRadius = 3.0f;
        [SerializeField] private float ExplosionPower = 1300.0f;
        
        protected new StructureInteractionType InteractionType = StructureInteractionType.Explode;

        private bool _didExplode;
        
        protected override void SetIdentifiers()
        {
            transform.name += " - Exploding";
        }
        
        protected override void SetVisualMaterial()
        {
            _meshRend.material = Resources.Load<Material>("Materials/Visual/brick_01");
        }

        protected override void HandleCollision(Collision coll)
        {
            if (coll.transform.CompareTag("Throwable") && !_didExplode)
            {
                // Debug.Log($"New exploding collision with: {coll.transform.tag}");

                Explode(coll);
            }
        }

        private void Explode(Collision coll)
        {
            Vector3 explosionPos = coll.transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, ExplosionRadius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(ExplosionPower, explosionPos, ExplosionRadius, 0f);
                }
            }

            _didExplode = true;
            gameObject.SetActive(false);
        }
    }
}