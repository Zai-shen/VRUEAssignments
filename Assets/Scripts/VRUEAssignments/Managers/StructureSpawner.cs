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
            float spawnDelay = 0.1f;

            float groundOffset = StructSize.y / 2f;
            float height = 0f;
            int pos = -1;

            for (int i = 0; i < Amount; i++)
            {
                Vector3 formationPos = default;
                float currentY = groundOffset + (StructSize.y * height);

                switch (++pos)
                {
                    case 0:
                        formationPos = _target.position + new Vector3(0, currentY, 0);
                        break;
                    case 1:
                        formationPos = _target.position + new Vector3(StructSize.x,currentY,StructSize.z);
                        break;
                    case 2:
                        formationPos = _target.position + new Vector3(0,currentY,StructSize.z * 2);
                        break;
                    case 3:
                        formationPos = _target.position + new Vector3(-StructSize.x,currentY,StructSize.z);
                        pos = -1;
                        height++;
                        break;
                }
                
                StructureInitializer temp = new StructureInitializer(formationPos, StructSize, structTypeToUse);
                temp.sTransform.SetParent(Container);
                structures.Add(temp.sTransform);
                
                yield return new WaitForSeconds(spawnDelay*=0.8f);
            }

            yield return null;
        }
    }
}
