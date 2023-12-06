using UnityEngine;

namespace VRUEAssignments.Map
{
    public class MapPart
    {
        public GameObject MapPartGo;
        private MapTile _mapTile;
        
        private MapTileType _type;
        private Grid<MapPart> _grid;
        private Vector3Int _position;
        
        public MapPart(MapTileType startType)
        {
            _type = startType;
        }
        
        public MapPart(MapTileType startType, Grid<MapPart> grid, Vector3Int position)
        {
            _type = startType;
            _grid = grid;
            _position = position;
        }
        
        public MapPart(MapTileType startType, Grid<MapPart> grid, int x, int y, int z)
        :this(startType,grid,new Vector3Int(x,y,z))
        { }

        public void ChangeType(MapTileType type)
        {
            _type = type;
            
            if (MapPartGo)
            {
                GameObject.Destroy(MapPartGo);
            }

            SetGameObject();
            
            _grid.TriggerGridObjectChanged(_position);
        }

        private void SetGameObject()
        {
            switch (_type)
            {
                case MapTileType.BASE:
                case MapTileType.PATH:
                    MapPartGo = new GameObject($"MapPartGO {_type.ToString()} - {_position.ToString()}");
                    _mapTile = MapPartGo.AddComponent<MapTile>();
                    _mapTile.SetType(_type);
                    _mapTile.Init(_position);
                    break;
                case MapTileType.EMPTY:
                default:
                    break;
            }
        }

        public override string ToString()
        {
            return //base.ToString() +
                _type.ToString();
        }
    }
}