using UnityEngine;

namespace VRUEAssignments.Map
{
    public enum MapTileType
    {
        EMPTY = 0,
        PATH = 1,
        BASE = 2
    }
    
    public class MapPart
    {
        public GameObject MapPartGo;
        
        private MapTileType _value;
        private Grid<MapPart> _grid;
        private int _x;
        private int _y;
        private int _z;
        
        public MapPart(MapTileType startValue)
        {
            _value = startValue;
        }
        
        public MapPart(MapTileType startValue, Grid<MapPart> grid, int x, int y, int z)
        {
            _value = startValue;
            _grid = grid;
            _x = x;
            _y = y;
            _z = z;
        }

        public void ChangeType(MapTileType type)
        {
            _value = type;

            switch (_value)
            {
                case MapTileType.BASE:
                    //Spawnbase
                    MapPartGo = new GameObject($"MapPartGO - x:{_x} y:{_y} z:{_z} _value:{_value.ToString()}");
                    break;
                case MapTileType.PATH:
                    //SPawnpath
                    MapPartGo = new GameObject($"MapPartGO - x:{_x} y:{_y} z:{_z} _value:{_value.ToString()}");
                    break;
                default:
                    break;
            }
            
            _grid.TriggerGridObjectChanged(_x, _y, _z);
        }

        public override string ToString()
        {
            return //base.ToString() +
                _value.ToString();
        }
    }
}