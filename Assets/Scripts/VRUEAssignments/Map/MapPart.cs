namespace VRUEAssignments.Map
{
    public class MapPart
    {
        private int _value;
        private Grid<MapPart> _grid;
        private int _x;
        private int _y;
        private int _z;
        
        public MapPart(int startValue)
        {
            _value = startValue;
        }
        
        public MapPart(int startValue, Grid<MapPart> grid, int x, int y, int z)
        {
            _value = startValue;
            _grid = grid;
            _x = x;
            _y = y;
            _z = z;
        }

        public void AddValue(int added)
        {
            _value += added;
            _grid.TriggerGridObjectChanged(_x, _y, _z);
        }

        public override string ToString()
        {
            return //base.ToString() +
                _value.ToString();
        }
    }
}