using System;
using UnityEngine;

namespace VRUEAssignments.Map
{
    public class MapPart
    {
        public GameObject MapPartGo;
        public MapTile MapTile;
        public MapTileSO MapTSo;
        public MapConnection MapCon;

        private Transform _mapTileContainer;
        
        private Grid<MapPart> _grid;
        private Vector3Int _gridPosition;
        private Vector3 _worldPosition;
        private float _cellSize;
        
        public MapPart(MapTileSO defaultMapTSo, Grid<MapPart> grid, Vector3Int gridPosition)
        {
            MapTSo = defaultMapTSo;
            _grid = grid;
            _gridPosition = gridPosition;
            _worldPosition = _grid.GetWorldPosition(_gridPosition);
            _cellSize = grid.CellSize;
            
            MapCon = new MapConnection(this);
        }
        
        public MapPart(MapTileSO defaultMapTSo, Grid<MapPart> grid, Vector3Int gridPosition, Transform mapTileContainer)
        :this(defaultMapTSo, grid, gridPosition)
        {
            SetParentContainer(mapTileContainer);
        }

        public MapPart GetSibling(Vector3Int gridPosition)
        {
            return _grid.GetGridObjectLocal(gridPosition);
        }

        public bool CouldConnectTo(MapPart mapPart)
        {
            MapPart prev = MapCon.ToMP;
            MapCon.ToMP = mapPart;
            bool didConnect = MapCon.CouldConnectTo(out float i);
            MapCon.ToMP = prev;
            return didConnect;
        }
        
        public bool ConnectTo(MapPart mapPart)
        {
            MapPart prev = MapCon.ToMP;
            MapCon.ToMP = mapPart;
            bool didConnect = MapCon.ConnectTo();
            if (didConnect)
            {
                MapTile.RotateY(MapCon.TargetRotation);
            }
            else
            {
                MapCon.ToMP = prev;
            }
            //Debug.Log($"ConnectTo: This {MapCon} to other {mapPart.MapCon}");
            
            return didConnect;
        }

        public void ChangeSilent(MapTileSO mapTileSo)
        {
            MapTSo = mapTileSo;
            MapCon.SetMobEntryExit();
        }
        
        public void Change(MapTileSO mapTileSo)
        {
            ChangeSilent(mapTileSo);
            SetGameObject();
            _grid.TriggerGridObjectChanged(_gridPosition);
        }

        public void SetGameObject()
        { 
            if (MapPartGo)
            {
                GameObject.Destroy(MapPartGo);
            }
            
            if (MapTSo.MapTType == MapTileType.EMPTY) return;
            
            MapPartGo = new GameObject($"MapPartGO {MapTSo.MapTType.ToString()} - {_gridPosition.ToString()}");
            if (_mapTileContainer != null) MapPartGo.transform.SetParent(_mapTileContainer);
            MapTile = MapPartGo.AddComponent<MapTile>();
            MapTile.MapTileSo = MapTSo;
            MapTile.SetSize(_cellSize);
            MapTile.Init();
            MapTile.SetRelativePosition(_worldPosition);
        }

        public void SetParentContainer(Transform container)
        {
            _mapTileContainer = container;
            if (MapPartGo != null)
            {
                MapPartGo.transform.SetParent(_mapTileContainer);
            }
        }

        public Vector3Int GetGridPosition()
        {
            return _gridPosition;
        }

        public void Clear()
        {
            MapCon.Clear();
            GameObject.Destroy(MapPartGo);
        }
        
        public override string ToString()
        {
            return $"{MapTSo.ToString()}";
        }
    }
}