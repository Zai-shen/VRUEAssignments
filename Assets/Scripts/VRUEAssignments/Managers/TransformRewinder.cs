using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VRUEAssignments.Managers
{
    public class TransformRewinder : MonoBehaviour
    {
        [SerializeField] private InputActionReference RecordButtonReference;
        private InputAction _recordButton;
        [SerializeField] private InputActionReference RewindButtonReference;
        private InputAction _rewindButton;
        
        [Tooltip("Frames to rewind - divide by 60 for seconds")]
        [Range(0,300)]
        [SerializeField] private int MaxFrames = 180;

        private List<GameObject> _transformGOs = new();
        private LinkedList<Dictionary<GameObject,Vector3>> _transformGOPositionMap = new();

        private bool _recording;
        private float _startingTime;
        private float _currentTime;

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
            _startingTime = Time.time;
            Debug.Log($"Starting to record at timestamp: {_startingTime}");
            Debug.Log($"Frames recorded: {_transformGOPositionMap.Count}");
            _recording = true;
        }
        
        private void Rewind(InputAction.CallbackContext ctx)
        {
            _startingTime = Time.time;
            Debug.Log($"Starting to rewind at timestamp: {_startingTime}");
            _recording = false;
        }

        
        private void Update()
        {
            if (_recording)
            {
                if (_transformGOPositionMap.Count >= MaxFrames)
                {
                    _transformGOPositionMap.RemoveFirst();
                }
                
                Dictionary<GameObject, Vector3> _currentPos = new();
                foreach (GameObject go in _transformGOs)
                {
                    _currentPos.Add(go,go.transform.position);
                }
                _transformGOPositionMap.AddLast(_currentPos);
            }
        }
    }
}