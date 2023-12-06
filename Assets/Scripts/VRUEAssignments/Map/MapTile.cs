using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VRUEAssignments.Map
{
    public class MapTile : MonoBehaviour
    {
        [SerializeField] private MapTileType _mapTileType;
        [SerializeField] private float _cellSize;
        
        private MapTileSO _mapTileSo;
        private GameObject _mesh;
        
        public void SetType(MapTileType mapTileType)
        {
            _mapTileType = mapTileType;

            GetTypeSO();
        }

        private void GetTypeSO()
        {
            switch (_mapTileType)
            {
                case MapTileType.PATH:
                    // TODO fix this to take corners too
                    _mapTileSo = MapTileSoLoader.GetRandomStraightPathSo();
                    break;
                case MapTileType.BASE:
                    _mapTileSo = MapTileSoLoader.GetRandomBaseSo();
                    break;
                case MapTileType.EMPTY:
                default:
                    Debug.LogWarning("MapTile: MapTileType should not be empty");
                    break;
            }
        }

        public void Init(Vector3 position)
        {
            _mesh = Instantiate(_mapTileSo.TilePrefab, transform);
            _mesh.name = _mapTileSo.Name;
            _mesh.transform.position = position + new Vector3(_cellSize,_cellSize,_cellSize) / 2f;
            _mesh.transform.localScale *= _cellSize;
        }
        
        public void SetSize(float size)
        {
            _cellSize = size;
        }
    }
}