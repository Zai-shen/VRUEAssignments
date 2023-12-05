using UnityEngine;
using UnityEngine.InputSystem;

namespace VRUEAssignments.Map
{
    public class GamingArea : MonoBehaviour
    {
        public bool DebugInEditor = false;
        public Camera TopCamera;

        private Grid<MapPart> _gamingAreaGrid;
        
        private void Start()
        {
            _gamingAreaGrid = new Grid<MapPart>(10, 10, 10, 1,
                (Grid<MapPart> mp, int x, int y, int z) => new MapPart(0, mp, x, y, z),
                transform.position);

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        _gamingAreaGrid.GetGridObject(x, y, z).AddValue(x + y + z);
                    }
                }
            }

            if (DebugInEditor)
            {
                TopCamera.gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector3 worldPos;
                if (DebugInEditor)
                {
                    worldPos = TopCamera.ScreenToWorldPoint(new Vector3(Mouse.current.position.value.x, Mouse.current.position.value.y, TopCamera.nearClipPlane));
                }
                else
                {
                    //TODO VR implementation 
                    worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.value.x, Mouse.current.position.value.y, Camera.main.nearClipPlane));
                }

                Debug.Log($"Mouse pos: {Mouse.current.position.value}");
                Debug.Log($"worldPos {worldPos.ToString()}");
                
                MapPart mPart = _gamingAreaGrid.GetGridObject(worldPos);
                mPart?.AddValue(5);
            }
        }
    }

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