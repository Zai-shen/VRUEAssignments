using UnityEngine;
using UnityEngine.InputSystem;

namespace VRUEAssignments.Map
{
    public class GamingArea : MonoBehaviour
    {
        public bool DebugInEditor;
        public Camera TopCamera;

        public Vector3 GridCenter = new Vector3(0, 0, 0);
        public Vector3Int GridSize = new Vector3Int(25, 1, 25);
        public float CellSize = 1f;

        public Transform MapTileContainer;
        public MapTileSO[] MapTileSos;

        private Grid<MapPart> _gamingAreaGrid;

        private void Start()
        {
            MapTileSoLoader.Init(MapTileSos);
            
            _gamingAreaGrid = new Grid<MapPart>(GridSize, CellSize,
                (mp, x, y, z) => new MapPart(MapTileType.EMPTY, mp, x, y, z, CellSize),
                GridCenter - GridSize / 2, true);
            
            
            MapPart part0 = _gamingAreaGrid.GetGridObject(GridCenter);
            part0.ChangeType(MapTileType.BASE);
            part0.SetMapTileContainer(MapTileContainer.transform);
            _gamingAreaGrid.GetGridObject(GridCenter + Vector3.left * 1 * CellSize).ChangeType(MapTileType.PATH);
            _gamingAreaGrid.GetGridObject(GridCenter + Vector3.left * 2 * CellSize).ChangeType(MapTileType.PATH);
            _gamingAreaGrid.GetGridObject(GridCenter + Vector3.left * 3 * CellSize).ChangeType(MapTileType.PATH);
            _gamingAreaGrid.GetGridObject(GridCenter + Vector3.left * 3 * CellSize + Vector3.forward * 1 * CellSize).ChangeType(MapTileType.PATH);
            
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
                    worldPos.y = GridCenter.y;
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