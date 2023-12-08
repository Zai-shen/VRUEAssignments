using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace VRUEAssignments.Map
{
    public class MapTile : MonoBehaviour
    {
        public MapTileSO MapTileSo;
        
        [SerializeField] private MapTileType _mapTileType;
        [SerializeField] private float _cellSize;
        
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
                    // TODO fix this
                    MapTileSo = Random.Range(0,2) == 0 ? 
                        MapResourceLoader.GetRandomStraightPathSo() : MapResourceLoader.GetRandomCornerPathSo();
                    break;
                case MapTileType.BASE:
                    MapTileSo = MapResourceLoader.GetRandomBaseSo();
                    break;
                case MapTileType.TELEPORT:
                    MapTileSo = MapResourceLoader.GetRandomTeleportSo();
                    break;
                case MapTileType.EMPTY:
                default:
                    Debug.LogWarning("MapTile: MapTileType should not be empty");
                    break;
            }
        }

        public void Init(Vector3 position)
        {
            _mesh = Instantiate(MapTileSo.TilePrefab, transform);
            _mesh.name = MapTileSo.Name;
            _mesh.transform.position = position + new Vector3(_cellSize,_cellSize,_cellSize) / 2f;
            _mesh.transform.localScale *= _cellSize;
        }
        
        public void SetSize(float size)
        {
            _cellSize = size;
        }
    }
}