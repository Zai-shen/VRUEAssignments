using System;
using System.Collections.Generic;
using Dreamteck.Splines;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using VRUEAssignments.NPCs.Enemies;
using VRUEAssignments.Utils;
using Random = UnityEngine.Random;

namespace VRUEAssignments.Map
{
    public class Map : UnitySingleton<Map>
    {
        public bool DebugInEditor;
        public Camera TopCamera;

        public Vector3 GridCenter = new Vector3(0, 0, 0);
        public Vector3Int GridSize = new Vector3Int(25, 1, 25);
        public float CellSize = 1f;

        public Transform MapTileContainer;
        public MapTileSO[] MapTileSos;

        public SplineComputer Spline;
        public Action NewTileSpawned;
        public Action MapInitialized;
        
        private RandomWeightedMapTileSO _randomWeightedMapTileSo;
        
        private Grid<MapPart> _gamingAreaGrid;

        private bool _isNextTileAvailable;

        protected override void Awake()
        {
            base.Awake();
            MapResourceLoader.Init(MapTileSos);
            _randomWeightedMapTileSo = GetComponent<RandomWeightedMapTileSO>();
        }

        private void Start()
        {
            _gamingAreaGrid = new Grid<MapPart>(GridSize, CellSize,
                (grid, pos) => new MapPart(MapResourceLoader.GetEmptySo(), grid, pos, MapTileContainer),
                GridCenter - GridSize / 2, DebugInEditor);
            
            _gamingAreaGrid.OnGridValueChanged += NotifyNeighbours;

            CreateMapTile(GridCenter, MapResourceLoader.GetBaseSo());
            CreateMapTile(GridCenter + Vector3.left, MapResourceLoader.GetRandomStraightPathSo());
            MapInitialized?.Invoke();
            
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
                // Debug.LogWarning($"Destroying {mPart.MapPartGo}");
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

        private void CreateMapTile(Vector3 worldPosition, MapTileSO mapTileSo)
        {
            MapPart mPart = _gamingAreaGrid.GetGridObjectWorld(worldPosition);
            if (mPart == null)
            {
                Debug.LogWarning($"Could not get MapPart at position {worldPosition}");
                return;
            }
            
            mPart.Change(mapTileSo);

            if (mapTileSo.MapTType is MapTileType.BASE or MapTileType.PATH)
            {
                AddToPath(mPart.MapTile.GetPathEntryToExit());
            }
        }

        private void AddToPath(List<Vector3> pathPositions)
        {
            if (pathPositions.Count == 0)
            {
                return;
            }
            
            SplinePoint[] points = Spline.GetPoints();
            int oldLength = points.Length;
            int newLength = oldLength + pathPositions.Count;
            SplinePoint[] newPoints = new SplinePoint[newLength];
            List<SplinePoint> pathPoints = new();

            foreach (Vector3 position in pathPositions)
            {
                SplinePoint temp = new SplinePoint();
                temp.position = position;
                temp.normal = Vector3.up;
                // temp.size = 1f;
                // temp.color = Color.white;
                
                pathPoints.Add(temp);
            }
            
            pathPoints.CopyTo(newPoints, 0);
            Array.Copy(points, 0, newPoints, pathPoints.Count, oldLength);
            
            Spline.SetPoints(newPoints);
            
            NewTileSpawned?.Invoke();
        }

        private void Update()
        {
            if (DebugInEditor)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Vector3 worldPos = TopCamera.ScreenToWorldPoint(
                        new Vector3(Mouse.current.position.value.x, Mouse.current.position.value.y,
                            TopCamera.nearClipPlane));
                    worldPos.y = GridCenter.y;
                    // Debug.Log($"Mouse pos: {Mouse.current.position.value}");
                    PlaceMapTile(worldPos);
                }
            }
        }

        public void PlaceMapTile(Vector3 worldPos)
        {
            //TODO VR implementation 
            // Debug.Log($"worldPos {worldPos.ToString()}");
            CreateMapTile(worldPos, ChooseMapTileSO(worldPos));
        }

        private MapTileSO ChooseMapTileSO(Vector3 worldPos)
        {
            MapPart mPart = _gamingAreaGrid.GetGridObjectWorld(worldPos);
            MapTileSO possibleTile = null;

            MapTileSO[] ordered = _randomWeightedMapTileSo.Generate();
            for (int index = 0; index < ordered.Length; index++)
            {
                MapTileSO currentTry = ordered[index];
                mPart.Change(currentTry);
                if (_isNextTileAvailable)
                {
                    return currentTry;
                }
            }
            
            Debug.LogWarning("No maptileso found that fits");
            return null;
        }
    }
}