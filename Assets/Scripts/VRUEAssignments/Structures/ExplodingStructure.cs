using UnityEngine;

namespace Unity.Template.VR.VRUEAssignments.Structures
{
    public class ExplodingStructure : Structure
    {
        protected new StructureInteractionType InteractionType = StructureInteractionType.Explode;

        protected override void SetName()
        {
            transform.name += " - Exploding";
        }
        
        protected override void SetVisualMaterial()
        {
            _meshRend.material = Resources.Load<Material>("Materials/Visual/brick_01");
        }

        protected override void HandleCollision(Collision coll)
        {
            Debug.Log($"New exploding collision with: {coll.transform.tag}");
        }
    }
}