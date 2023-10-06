using Unity.VisualScripting;
using UnityEngine;

namespace Unity.Template.VR.VRUEAssignments.Structures
{
    public class StructureInitializer
    {
        public Transform sTransform;
        private PrimitiveType structTypeToUse = PrimitiveType.Cube;
        
        public StructureInitializer()
        {
            Init(structTypeToUse);
        }
        
        public StructureInitializer(PrimitiveType structType)
        {
            structTypeToUse = structType;
            Init(structTypeToUse);
        }

        public StructureInitializer(Vector3 position, Vector3 size = default, PrimitiveType structType = PrimitiveType.Cube)
        {
            Init(structType);
            sTransform.localScale = size == default ? Vector3.one : size;
            sTransform.position = position;
        }
        
        private void Init(PrimitiveType structType)
        {
            sTransform = GameObject.CreatePrimitive(structType).transform;

            AddStructureComponent();
        }

        private void AddStructureComponent()
        {
            int percentage = Random.Range(1, 101);

            switch (percentage)
            {
                case > 50:
                    sTransform.AddComponent<Structure>();
                    break;
                case > 25:
                    sTransform.AddComponent<MassiveStructure>();
                    break;
                case > 0:
                    sTransform.AddComponent<ExplodingStructure>();
                    break;
            }
        }
    }
}