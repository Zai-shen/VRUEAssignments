﻿using UnityEngine;

namespace VRUEAssignments.Map
{
    public class MapPart
    {
        public GameObject MapPartGo;
        private MapTile _mapTile;
        private Transform _mapTileContainer;
        
        private MapTileType _type;
        private Grid<MapPart> _grid;
        private Vector3Int _gridPosition;
        private Vector3 _worldPosition;
        private float _cellSize;
        
        public MapPart(MapTileType startType, Grid<MapPart> grid, Vector3Int gridPosition, float cellSize = 1f)
        {
            _type = startType;
            _grid = grid;
            _gridPosition = gridPosition;
            _worldPosition = _grid.GetWorldPosition(_gridPosition);
            _cellSize = cellSize;
        }

        public MapTileType GetTileType()
        {
            return _type;
        }

        public void ChangeType(MapTileType type)
        {
            _type = type;
            
            if (MapPartGo)
            {
                GameObject.Destroy(MapPartGo);
            }

            SetGameObject();
            
            _grid.TriggerGridObjectChanged(_gridPosition);
        }

        private void SetGameObject()
        {
            switch (_type)
            {
                case MapTileType.BASE:
                case MapTileType.PATH:
                    MapPartGo = new GameObject($"MapPartGO {_type.ToString()} - {_gridPosition.ToString()}");
                    if (_mapTileContainer != null) MapPartGo.transform.SetParent(_mapTileContainer);
                    _mapTile = MapPartGo.AddComponent<MapTile>();
                    _mapTile.SetType(_type);
                    _mapTile.SetSize(_cellSize);
                    _mapTile.Init(_worldPosition);
                    break;
                case MapTileType.EMPTY:
                default:
                    break;
            }
        }

        public void SetParentContainer(Transform container)
        {
            _mapTileContainer = container;
            if (MapPartGo != null)
            {
                MapPartGo.transform.SetParent(_mapTileContainer);
            }
        }
        
        public override string ToString()
        {
            return $"{_type.ToString()}";
        }
    }
}