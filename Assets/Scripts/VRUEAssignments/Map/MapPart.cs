using UnityEngine;

namespace VRUEAssignments.Map
{
    public class MapPart
    {
        public GameObject MapPartGo;
        private MapTile _mapTile;
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
            switch (MTType)
            {
                case MapTileType.BASE:
                case MapTileType.PATH:
                case MapTileType.TELEPORT:
                    MapPartGo = new GameObject($"MapPartGO {MTType.ToString()} - {_gridPosition.ToString()}");
                    if (_mapTileContainer != null) MapPartGo.transform.SetParent(_mapTileContainer);
                    _mapTile = MapPartGo.AddComponent<MapTile>();
                    _mapTile.SetType(MTType);
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
            return $"{MTType.ToString()}";
        }
    }
}