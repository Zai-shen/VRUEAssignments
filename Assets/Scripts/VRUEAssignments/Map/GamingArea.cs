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
            _gamingAreaGrid = new Grid<MapPart>(10, 10, 1,
                (Grid<MapPart> mp, int x, int y) => new MapPart(0, mp, x, y),
                transform.position);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _gamingAreaGrid.GetGridObject(i,j).AddValue(i+j);
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
        

        public MapPart(int startValue)
        {
            _value = startValue;
        }
        
        public MapPart(int startValue, Grid<MapPart> grid, int x, int y)
        {
            _value = startValue;
            _grid = grid;
            _x = x;
            _y = y;
        }

        public void AddValue(int added)
        {
            _value += added;
            _grid.TriggerGridObjectChanged(_x, _y);
        }

        public override string ToString()
        {
            return //base.ToString() +
                   _value.ToString();
        }
    }
}