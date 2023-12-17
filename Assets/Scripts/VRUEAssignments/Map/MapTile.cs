using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace VRUEAssignments.Map
{
    public class MapTile : MonoBehaviour
    {
        public MapTileSO MapTileSo;

        [SerializeField] private float _cellSize;
        [SerializeField] private string _navigationName = "Navigation";
        [SerializeField] private string _entryName = "Entry";
        [SerializeField] private string _exitName = "Exit";
        [SerializeField] private string _midName = "Mid";
        
        private GameObject _mesh;
        
        public void Init()
        {
            _mesh = Instantiate(MapTileSo.TilePrefab, transform);
            _mesh.name = MapTileSo.Name;
            _mesh.transform.localScale *= _cellSize;
        }

        public void SetAbsolutePosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetRelativePosition(Vector3 position)
        {
            SetAbsolutePosition(position + new Vector3(_cellSize,_cellSize,_cellSize) / 2f);
        }

        public void RotateY(float rotation)
        {
            transform.Rotate(Vector3.up,  rotation);
        }
        
        public void SetSize(float size)
        {
            _cellSize = size;
        }

        public List<Vector3> GetPathEntryToExit()
        {
            List<Vector3> positions = new();

            Transform child = transform.GetChild(0);
            if (child == null) return positions;

            Transform navigation = child.Find(_navigationName);
            if (navigation == null) return positions;

            Transform entry = navigation.Find(_entryName);
            if (entry != null) positions.Add(entry.position);

            Transform mid = navigation.Find(_midName);
            if (mid != null) positions.Add(mid.position);

            Transform exit = navigation.Find(_exitName);
            if (exit != null) positions.Add(exit.position);
            
            return positions;
        }
    }
}