using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VRUEAssignments.Map
{
    public class Map : MonoBehaviour
    {
        public bool DebugInEditor;
        public Camera TopCamera;

        public Vector3 GridCenter = new Vector3(0, 0, 0);
        public Vector3Int GridSize = new Vector3Int(25, 1, 25);
        public float CellSize = 1f;

        public Transform MapTileContainer;
        public MapTileSO[] MapTileSos;
        
        private Grid<MapPart> _gamingAreaGrid;

        private void Awake()
        {
            MapResourceLoader.Init(MapTileSos);
        }

        private void Start()
        {
            _gamingAreaGrid = new Grid<MapPart>(GridSize, CellSize,
                (mp,pos) => new MapPart(MapTileType.EMPTY, mp, pos, MapTileContainer ,CellSize),
                GridCenter - GridSize / 2, true);
            
            _gamingAreaGrid.OnGridValueChanged += NotifyNeighbours;

            CreateMapTile(GridCenter, MapTileType.BASE);
            CreateMapTile(GridCenter + Vector3.left * 1 * CellSize, MapTileType.PATH);
            // CreateMapTile(GridCenter + Vector3.left * 2 * CellSize, MapTileType.PATH);
            // CreateMapTile(GridCenter + Vector3.left * 2 * CellSize, MapTileType.PATH);
            // CreateMapTile(GridCenter + Vector3.left * 3 * CellSize + Vector3.forward * 1 * CellSize, MapTileType.PATH);
            
            if (DebugInEditor)
            {
                TopCamera.gameObject.SetActive(true);
            }
        }

        private void NotifyNeighbours(Vector3Int position)
        {
            MapPart mPart = _gamingAreaGrid.GetGridObjectLocal(position);
            
            if (mPart.MTType is MapTileType.TELEPORT or MapTileType.EMPTY) return;

            bool didConnect = false;
            List<MapPart> neighbours = GetNeighbours(position);
            foreach (MapPart mP in neighbours)
            {
                if (mP.MTType == MapTileType.EMPTY)
                {
                    mP.ChangeType(MapTileType.TELEPORT);
                }
                else if (mP.MTType == MapTileType.TELEPORT)
                {
                    continue;
                }else if (!didConnect)
                {
                    didConnect = mPart.MapTile.TryConnectTo(mP.MapTile);
                }
            }
        }

        private List<MapPart> GetNeighbours(Vector3Int position)
        {
            List<MapPart> neighbours = new();
            AddToListIfNotNull(neighbours, position + Vector3Int.left);
            AddToListIfNotNull(neighbours, position + Vector3Int.forward);
            AddToListIfNotNull(neighbours, position + Vector3Int.right);
            AddToListIfNotNull(neighbours, position + Vector3Int.back);

            return neighbours;
        }

        private void AddToListIfNotNull(List<MapPart> neighbours, Vector3Int position)
        {
            MapPart temp =_gamingAreaGrid.GetGridObjectLocal(position);
            if (temp == null) return;
            neighbours.Add(temp);
        }

        private MapPart CreateMapTile(Vector3 worldPosition, MapTileType mapTileType)
        {
            MapPart mPart = _gamingAreaGrid.GetGridObjectWorld(worldPosition);
            if (mPart == null)
            {
                Debug.LogWarning($"Could not get MapPart at position {worldPosition}");
                return default;
            }
            
            mPart.ChangeType(mapTileType);
            mPart.SetParentContainer(MapTileContainer.transform);
            
            return mPart;
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

                // Debug.Log($"Mouse pos: {Mouse.current.position.value}");
                // Debug.Log($"worldPos {worldPos.ToString()}");

                CreateMapTile(worldPos, MapTileType.PATH);
            }
        }
    }
}