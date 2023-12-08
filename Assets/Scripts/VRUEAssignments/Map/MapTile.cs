using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace VRUEAssignments.Map
{
    public class MapTile : MonoBehaviour
    {
        public MapTileSO MapTileSo;
        public XZCoords MobEntry;
        public XZCoords MobExit;
        
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
                    // MapTileSo = MapResourceLoader.GetRandomCornerPathSo();
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

            MobEntry = MapTileSo.MobEntry;
            MobExit = MapTileSo.MobExit;
        }

        public void Init(Vector3 position)
        {
            _mesh = Instantiate(MapTileSo.TilePrefab, transform);
            _mesh.name = MapTileSo.Name;
            _mesh.transform.localScale *= _cellSize;
            transform.position = position + new Vector3(_cellSize,_cellSize,_cellSize) / 2f;
        }
        
        public void SetSize(float size)
        {
            _cellSize = size;
        }

        public bool TryConnectTo(MapTile other)
        {
            bool clockWise = Random.value > 0.5f;

            for (int i = 0; i < 4; i++)
            {
                if (IsConnectable(this.MobExit, other.MobEntry, other))
                {
                    Debug.Log($"Connecting this {gameObject.name} with MobExit {MobExit} to {other.gameObject.name} with Mobentry {other.MobEntry}");
                    transform.Rotate(Vector3.up, (clockWise ? 90f : -90f) * i);
                    return true;
                }
                
                if (clockWise)
                {
                    RotateClockWise();
                }
                else
                {
                    RotateCounterClockWise();
                }
            }

            Debug.Log($"Thispos{transform.position} otherpos{other.transform.position}");
            Debug.Log($"Could NOT connect this {gameObject.name} with MobExit {MobExit} to {other.gameObject.name} with Mobentry {other.MobEntry}");
            return false;
        }

        private void RotateCounterClockWise()
        {
            RotateXZCoordinateCCW(ref MobEntry);
            RotateXZCoordinateCCW(ref MobExit);
        }
        
        private void RotateClockWise()
        {
            RotateXZCoordinateCW(ref MobEntry);
            RotateXZCoordinateCW(ref MobExit);
        }

        private void RotateXZCoordinateCW(ref XZCoords coord)
        {
            switch (coord)
            {
                case XZCoords.UP:
                    coord = XZCoords.RIGHT;
                    break;
                case XZCoords.RIGHT:
                    coord = XZCoords.DOWN;
                    break;
                case XZCoords.DOWN:
                    coord = XZCoords.LEFT;
                    break;
                case XZCoords.LEFT:
                    coord = XZCoords.UP;
                    break;
                case XZCoords.CENTER:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(coord), coord, null);
            }
        }
        
        private void RotateXZCoordinateCCW(ref XZCoords coord)
        {
            switch (coord)
            {
                case XZCoords.UP:
                    coord = XZCoords.LEFT;
                    break;
                case XZCoords.RIGHT:
                    coord = XZCoords.UP;
                    break;
                case XZCoords.DOWN:
                    coord = XZCoords.RIGHT;
                    break;
                case XZCoords.LEFT:
                    coord = XZCoords.DOWN;
                    break;
                case XZCoords.CENTER:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(coord), coord, null);
            }
        }

        private bool IsConnectable(XZCoords thisExit, XZCoords otherEntry, MapTile other)
        {
            var thisPos = transform.position;
            var otherPos = other.transform.position;

            return (thisExit == XZCoords.RIGHT && otherEntry == XZCoords.LEFT) && thisPos.x < otherPos.x ||
                   (thisExit == XZCoords.LEFT && otherEntry == XZCoords.RIGHT) && thisPos.x > otherPos.x ||
                   (thisExit == XZCoords.UP && otherEntry == XZCoords.DOWN) && thisPos.z < otherPos.z ||
                   (thisExit == XZCoords.DOWN && otherEntry == XZCoords.UP) && thisPos.z > otherPos.z;
        }
    }
}