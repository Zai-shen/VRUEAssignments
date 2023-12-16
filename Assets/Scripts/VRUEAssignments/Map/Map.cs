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

        private bool _isNextTileAvailable;
        
        private void Awake()
        {
            MapResourceLoader.Init(MapTileSos);
        }

        private void Start()
        {
            _gamingAreaGrid = new Grid<MapPart>(GridSize, CellSize,
                (grid, pos) => new MapPart(MapResourceLoader.GetEmptySo(), grid, pos, MapTileContainer),
                GridCenter - GridSize / 2, true);
            
            _gamingAreaGrid.OnGridValueChanged += NotifyNeighbours;

            CreateMapTile(GridCenter, MapResourceLoader.GetBaseSo());
            // CreateMapTile(GridCenter + Vector3.left * 1 * CellSize, MapResourceLoader.GetNextSo());
            // CreateMapTile(GridCenter + Vector3.left * 1 * CellSize, MapResourceLoader.GetRandomStraightPathSo());
            // CreateMapTile(GridCenter + Vector3.left * 2 * CellSize, MapResourceLoader.GetRandomCornerRightPathSo());
            // CreateMapTile(GridCenter + Vector3.left * 2 * CellSize + Vector3.forward * 1 * CellSize, MapResourceLoader.GetRandomCornerRightPathSo());
            // CreateMapTile(GridCenter + Vector3.left * 1 * CellSize + Vector3.forward * 1 * CellSize, MapResourceLoader.GetRandomCornerLeftPathSo());
            
            if (DebugInEditor)
            {
                TopCamera.gameObject.SetActive(true);
            }
        }

        private void NotifyNeighbours(Vector3Int position)
        {
            MapPart mPart = _gamingAreaGrid.GetGridObjectLocal(position);
            
            if (mPart.MapTSo.MapTType is MapTileType.TELEPORT or MapTileType.EMPTY or MapTileType.NEXT) return;

            _isNextTileAvailable = false;
            bool didConnect = false;
            
            List<MapPart> neighbours = GetNeighbours(position);
            foreach (MapPart mP in neighbours)
            {
                if (mP.MapTSo.MapTType == MapTileType.EMPTY)
                {
                    mP.ChangeSilent(MapResourceLoader.GetRandomTeleportSo());
                    mP.SetGameObject();
                }
                else if (mP.MapTSo.MapTType == MapTileType.TELEPORT)
                {
                    continue;
                }else if (!didConnect)
                {
                    didConnect = mPart.ConnectTo(mP);
                }
            }

            foreach (MapPart mP in neighbours)
            {
                if (mP.MapTSo.MapTType == MapTileType.TELEPORT)
                {
                    if (mP.CouldConnectTo(mPart))
                    {
                        mP.ChangeSilent(MapResourceLoader.GetNextSo());
                        mP.SetGameObject();
                        _isNextTileAvailable = true;
                        break;
                    }
                }
            }

            if (mPart.MapTSo.MapTType == MapTileType.BASE) return;

            if (!_isNextTileAvailable && !didConnect)
            {
                Debug.LogWarning($"Destroying {mPart.MapPartGo}");
                mPart.Clear();
                return;
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

        private bool CreateMapTile(Vector3 worldPosition, MapTileSO mapTileSo)
        {
            MapPart mPart = _gamingAreaGrid.GetGridObjectWorld(worldPosition);
            if (mPart == null)
            {
                Debug.LogWarning($"Could not get MapPart at position {worldPosition}");
                return default;
            }
            
            mPart.Change(mapTileSo);
            
            return mPart.MapCon.IsConnectedWithTO();
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

                CreateMapTile(worldPos, ChooseMapTileSO(worldPos));
            }
        }

        private MapTileSO ChooseMapTileSO(Vector3 worldPos)
        {
            MapPart mPart = _gamingAreaGrid.GetGridObjectWorld(worldPos);
            MapTileSO possibleTile = null;
            
            
            
            mPart.Change(MapResourceLoader.GetRandomCornerRightPathSo());
            if (_isNextTileAvailable)
            {
                return MapResourceLoader.GetRandomCornerRightPathSo();
            }

            mPart.Change(MapResourceLoader.GetRandomStraightPathSo());
            if (_isNextTileAvailable)
            {
                return MapResourceLoader.GetRandomStraightPathSo();
            }

            mPart.Change(MapResourceLoader.GetRandomCornerLeftPathSo());
            if (_isNextTileAvailable)
            {
                return MapResourceLoader.GetRandomCornerLeftPathSo();
            }

            Debug.LogWarning("No maptileso found that fits");
            return null;

            // List<MapPart> neighbours = GetNeighbours(mPart.GetGridPosition());
            //
            // MapTileSO straightOnly = MapResourceLoader.GetRandomStraightPathSo();
            // MapTileSO rightOnly = MapResourceLoader.GetRandomCornerRightPathSo();
            //
            //
            // bool didChange = false;
            //
            // mPart.ChangeSilent(rightOnly);
            // foreach (MapPart mP in neighbours)
            // {
            //     Debug.Log(mP.MapTSo);
            //     if (mP.MapTSo.MapTType is MapTileType.TELEPORT or MapTileType.EMPTY)
            //     {
            //         if (mP.CouldConnectTo(mPart)) 
            //         {
            //             mP.Change(MapResourceLoader.GetNextSo());
            //             didChange = true;
            //             possibleTile = rightOnly;
            //             break;
            //         }
            //     }
            // }
            //
            // if (!didChange)
            // {
            //     mPart.ChangeSilent(straightOnly);
            //
            //     foreach (MapPart mP in neighbours)
            //     {
            //         if (mP.MapTSo.MapTType is MapTileType.TELEPORT or MapTileType.EMPTY)
            //         {
            //             if (mP.CouldConnectTo(mPart))
            //             {
            //                 mP.Change(MapResourceLoader.GetNextSo());
            //                 didChange = true;
            //                 possibleTile = straightOnly;
            //                 break;
            //             }
            //         }
            //     }
            // }
            
            return possibleTile;
            // return MapResourceLoader.GetRandomPath();
        }
    }
}