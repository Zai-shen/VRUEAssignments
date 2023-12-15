using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace VRUEAssignments.Map
{
    public class MapTile : MonoBehaviour
    {
        public MapTileSO MapTileSo;

        [SerializeField] private float _cellSize;
        
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
    }
}