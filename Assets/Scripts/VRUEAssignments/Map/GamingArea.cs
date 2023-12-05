using UnityEngine;
using UnityEngine.InputSystem;

namespace VRUEAssignments.Map
{
    public class GamingArea : MonoBehaviour
    {
        public bool DebugInEditor = false;
        public Camera TopCamera;

        public Vector3Int GridSize = new Vector3Int(25, 1, 25);
        
        private Grid<MapPart> _gamingAreaGrid;
        
        private void Start()
        {
            _gamingAreaGrid = new Grid<MapPart>(GridSize, 1,
                (Grid<MapPart> mp, int x, int y, int z) => new MapPart(MapTileType.EMPTY, mp, x, y, z),
                transform.position - GridSize / 2, true);
            
            _gamingAreaGrid.GetGridObject(transform.position ).ChangeType(MapTileType.BASE);
            _gamingAreaGrid.GetGridObject(transform.position  + Vector3Int.left).ChangeType(MapTileType.PATH);
            _gamingAreaGrid.GetGridObject(transform.position  + Vector3Int.left * 2 ).ChangeType(MapTileType.PATH);
            
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
                mPart?.ChangeType(MapTileType.PATH);
            }
        }
    }
}