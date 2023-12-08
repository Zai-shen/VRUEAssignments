using System;
using UnityEngine;

namespace VRUEAssignments.Map
{
    public class MapPart
    {
        public GameObject MapPartGo;
        public MapTile MapTile;
        private Transform _mapTileContainer;
        
        public MapTileType MTType;
        private Grid<MapPart> _grid;
        private Vector3Int _gridPosition;
        private Vector3 _worldPosition;
        private float _cellSize;
        
        public MapPart(MapTileType startMTType, Grid<MapPart> grid, Vector3Int gridPosition, float cellSize = 1f)
        {
            MTType = startMTType;
            _grid = grid;
            _gridPosition = gridPosition;
            _worldPosition = _grid.GetWorldPosition(_gridPosition);
            _cellSize = cellSize;
        }
        
        public MapPart(MapTileType startMTType, Grid<MapPart> grid, Vector3Int gridPosition, Transform mapTileContainer, float cellSize = 1f)
        :this(startMTType, grid, gridPosition,cellSize)
        {
            SetParentContainer(mapTileContainer);
        }

        public void ChangeType(MapTileType type)
        {
            MTType = type;
            
            if (MapPartGo)
            {
                GameObject.Destroy(MapPartGo);
            }

            SetGameObject();
            
            _grid.TriggerGridObjectChanged(_gridPosition);
        }

        private void SetGameObject()
        {
            if (MTType == MapTileType.EMPTY) return;
            
            MapPartGo = new GameObject($"MapPartGO {MTType.ToString()} - {_gridPosition.ToString()}");
            if (_mapTileContainer != null) MapPartGo.transform.SetParent(_mapTileContainer);
            MapTile = MapPartGo.AddComponent<MapTile>();
            MapTile.SetType(MTType);
            MapTile.SetSize(_cellSize);
            MapTile.Init(_worldPosition);
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
            return $"{MTType.ToString()}";
        }
    }
}