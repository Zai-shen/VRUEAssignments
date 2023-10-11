using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VRUEAssignments.Utils;

namespace VRUEAssignments.Managers
{
    public class TransformRewinder : MonoBehaviour
    {
        [SerializeField] private InputActionReference RecordButtonReference;
        private InputAction _recordButton;
        [SerializeField] private InputActionReference RewindButtonReference;
        private InputAction _rewindButton;
        
        [Tooltip("Frames to rewind - divide by 60 for seconds")]
        [Range(0,600)]
        [SerializeField] private int MaxFrames = 300;

        private List<GameObject> _transformGOs = new();
        private LinkedList<List<Destination>> _transformDestinations = new();

        private bool _recording;

        private StructureSpawner _structureSpawner;

        private void Start()
        {
            _structureSpawner = StructureSpawner.Instance;
            _structureSpawner.OnSpawned += GetTransforms;

            _recordButton = RecordButtonReference.ToInputAction();
            _rewindButton = RewindButtonReference.ToInputAction();

            _recordButton.performed += Record;
            _rewindButton.performed += Rewind;
        }

        private void GetTransforms()
        {
            _transformGOs = _structureSpawner.InstantiatedObjects;
            _recording = true;
        }

        private void Record(InputAction.CallbackContext ctx)
        {
            _recording = !_recording;
        }
        
        private void Rewind(InputAction.CallbackContext ctx)
        {
            _recording = false;

            StartCoroutine(Rewind());
        }

        private IEnumerator Rewind()
        {
            if (_transformDestinations.Count == 0 || _transformDestinations == null) yield break;
            
            List<Destination> currentDestinations = _transformDestinations.Last.Value;
            List<Rigidbody> rigidbodies = new();
            foreach (Destination destination in currentDestinations)
            {
                rigidbodies.Add(destination.Go.GetComponent<Rigidbody>());
            }
            
            EnableCollisions(rigidbodies, false);
            
            while (_transformDestinations.Count > 0 && !_recording)
            {
                currentDestinations = _transformDestinations.Last.Value;
                SetTransformDestinations(currentDestinations);

                _transformDestinations.RemoveLast();
                yield return null;
            }

            EnableCollisions(rigidbodies, true);

            _recording = true;
            yield return null;
        }

        private static void EnableCollisions(List<Rigidbody> rigidbodies, bool state)
        {
            foreach (Rigidbody rb in rigidbodies)
            {
                rb.isKinematic = !state;
            }
        }

        private static void SetTransformDestinations(List<Destination> currentDestinations)
        {
            foreach (Destination destination in currentDestinations)
            {
                Transform goTrans = destination.Go.transform;
                goTrans.position = destination.Position;
                goTrans.rotation = destination.Rotation;
            }
        }

        private void Update()
        {
            if (_recording)
            {
                LimitMemoryOverflow();
                SaveDestinations();
            }
        }

        private void LimitMemoryOverflow()
        {
            if (_transformDestinations.Count >= MaxFrames)
            {
                _transformDestinations.RemoveFirst();
            }
        }

        private void SaveDestinations()
        {
            List<Destination> currentDestinations = new();
            foreach (GameObject go in _transformGOs)
            {
                Destination currentDestination = new Destination(go, go.transform.position, go.transform.rotation);
                currentDestinations.Add(currentDestination);
            }

            _transformDestinations.AddLast(currentDestinations);
        }
    }
}