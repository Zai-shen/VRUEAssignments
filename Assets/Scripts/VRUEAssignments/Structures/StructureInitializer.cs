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
            InitTransform(structTypeToUse);
            AddStructureComponent();
        }
        
        public StructureInitializer(PrimitiveType structType)
        {
            structTypeToUse = structType;
            InitTransform(structTypeToUse);
            AddStructureComponent();
        }

        public StructureInitializer(Vector3 position, Vector3 size, PrimitiveType structType = PrimitiveType.Cube)
        {
            InitTransform(position,size,structType);
            AddStructureComponent();
        }
        
        private void InitTransform(PrimitiveType structType)
        {
            sTransform = GameObject.CreatePrimitive(structType).transform;
        }
        
        private void InitTransform(Vector3 position, Vector3 size, PrimitiveType structType)
        {
            sTransform = GameObject.CreatePrimitive(structType).transform;
            sTransform.localScale = size == default ? Vector3.one : size;
            sTransform.position = position;
        }

        private void AddStructureComponent()
        {
            int percentage = Random.Range(1, 101);

            switch (percentage)
            {
                case > 35:
                    sTransform.AddComponent<Structure>();
                    break;
                case > 10:
                    sTransform.AddComponent<MassiveStructure>();
                    break;
                case > 0:
                    sTransform.AddComponent<ExplodingStructure>();
                    break;
            }
        }
    }
}