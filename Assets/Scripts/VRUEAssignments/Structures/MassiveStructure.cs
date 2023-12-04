using UnityEngine;

namespace VRUEAssignments.Structures
{
    public class MassiveStructure : Structure
    {
        protected new StructureInteractionType InteractionType = StructureInteractionType.Massive;

        protected override void SetIdentifiers()
        {
            transform.name += " - Massive";
        }
        
        protected override void SetRigidbody()
        {
            base.SetRigidbody();
            _rb.mass *= 2f;
        }

        protected override void SetVisualMaterial()
        {
            _meshRend.material = Resources.Load<Material>("Materials/Visual/ground_02");
        }
        
        protected override void SetPhysicalMaterial()
        {
            _coll.material = Resources.Load<PhysicMaterial>("Materials/Physical/StructureMassive");
        }

        protected override void HandleCollision(Collision coll)
        {
            if (coll.transform.CompareTag("Throwable"))
            {
                // Debug.Log($"New massive collision with: {coll.transform.tag}");
            }
        }
    }
}