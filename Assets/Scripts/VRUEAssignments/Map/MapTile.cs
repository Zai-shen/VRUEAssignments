using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VRUEAssignments.Map
{
    public class MapTile : MonoBehaviour
    {
        [FormerlySerializedAs("MapTType")] [SerializeField] private MapTileType _mapTileType;
        
        private MapTileSO _mapTileSo;
        private GameObject _mesh;

        private void Start()
        {
        }

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
                    _mapTileSo = MapTileSOLoader.GetRandomStraightPathSO();
                    break;
                case MapTileType.BASE:
                    _mapTileSo = MapTileSOLoader.GetRandomBaseSO();
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
            _mesh.transform.position = position;
        }
    }
}