using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Template.VR.VRUEAssignments.Structures;
using UnityEngine;
using UnityEngine.Serialization;
using VRUEAssignments.Utils;

namespace VRUEAssignments.Managers
{
    public class ThrowableSpawner : UnitySingleton<ThrowableSpawner>
    {
        [SerializeField] private GameObject[] ThrowablePrefabs;
        [SerializeField] private GameObject SpawnParticleEffect;

        [SerializeField] private Transform Container;
        [SerializeField] private Transform Target;
        
        private List<GameObject> _instantiatedObjects = new();
        
        private int _currentThrowable = 0;

        public void SetCurrentThrowable(ThrowableType arrayIndex)
        {
            _currentThrowable = (int)arrayIndex;
        }
        
        public void SetCurrentThrowable(int arrayIndex)
        {
            _currentThrowable = arrayIndex;
        }
        
        public void SpawnThrowable(int throwableArrayIndex)
        {
            GameObject throwable = Instantiate(ThrowablePrefabs[throwableArrayIndex], Target.position, Quaternion.identity);
            throwable.transform.SetParent(Container);
            _instantiatedObjects.Add(throwable);

            Instantiate(SpawnParticleEffect, Target.position, SpawnParticleEffect.transform.rotation);
        }
        
        public void SpawnThrowable()
        {
            SpawnThrowable(_currentThrowable);
        }

    }
}
