using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Template.VR.VRUEAssignments.Structures;
using UnityEngine;
using VRUEAssignments.Utils;

namespace VRUEAssignments.Managers
{
    public class StructureSpawner : UnitySingleton<StructureSpawner>
    {
        [SerializeField] private StructureType StructType = StructureType.Cube;
        private PrimitiveType structTypeToUse;
        
        [SerializeField][Range(1,100)] private int Amount;
        [SerializeField] private Vector3 StructSize = Vector3.one;

        [SerializeField] private Transform Container;
        private Transform _target;
        
        protected override void Awake()
        {
            base.Awake();
            GetPrimitiveType();
            _target = Container;
        }

        private void GetPrimitiveType()
        {
            switch (StructType)
            {
                case StructureType.Cube:
                    structTypeToUse = PrimitiveType.Cube;
                    break;
                case StructureType.Sphere:
                    structTypeToUse = PrimitiveType.Sphere;
                    break;
                default:
                    structTypeToUse = PrimitiveType.Cube;
                    break;
            }
        }

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            List<Transform> structures = new();
            
            float groundOffset = StructSize.y / 2f;
            float height = 0f;
            int pos = -1;

            for (int i = 0; i < Amount; i++)
            {
                StructureInitializer temp = null;
                float currentY = groundOffset + (StructSize.y * height);
                pos++;
                switch (pos)
                {
                    case 0:
                        temp = new StructureInitializer(_target.position + new Vector3(0,currentY,0), StructSize, structTypeToUse);
                        break;
                    case 1:
                        temp = new StructureInitializer(_target.position + new Vector3(StructSize.x,currentY,StructSize.z), StructSize, structTypeToUse);
                        break;
                    case 2:
                        temp = new StructureInitializer(_target.position + new Vector3(0,currentY,StructSize.z * 2), StructSize, structTypeToUse);
                        break;
                    case 3:
                        temp = new StructureInitializer(_target.position + new Vector3(-StructSize.x,currentY,StructSize.z), StructSize, structTypeToUse);
                        pos = -1;
                        height++;
                        break;
                }
                
                temp.sTransform.SetParent(Container);
                structures.Add(temp.sTransform);
                
                yield return new WaitForSeconds(0.05f);
            }

            yield return null;
        }
    }
}
