using System;
using TMPro;
using UnityEngine;

namespace VRUEAssignments.Map
{
    public class Grid<T>
    {
        public Action<Vector3Int> OnGridValueChanged;

        private Vector3 _originPosition;

        private Vector3Int _gridSize;
        private T[,,] _gridArray;
        private float _cellSize;

        private bool _debug;
        private GameObject[,,] _gridDebugArray;

        public Grid(Vector3Int size, float cellSize, Func<Grid<T>, Vector3Int, T> CreateGridObject,
            Vector3 originPosition = default(Vector3), bool debug = false)
        {
            _originPosition = originPosition;
            _gridSize = size;
            _gridArray = new T[_gridSize.x, _gridSize.y, _gridSize.z];
            _cellSize = cellSize;
            _debug = debug;
            
            Init(CreateGridObject);
        }

        private void Init(Func<Grid<T>, Vector3Int, T> CreateGridObject)
        {
            FillGridWithDefault(CreateGridObject);
            
            if (_debug)
            {
                _gridDebugArray = new GameObject[_gridSize.x, _gridSize.y, _gridSize.z];
                OnGridValueChanged += UpdateDebugText;
                FillGridWithDebug();
                DrawDebugLines();
            }
        }

        private void FillGridWithDefault(Func<Grid<T>, Vector3Int, T> CreateGridObject)
        {
            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    for (int z = 0; z < _gridArray.GetLength(2); z++)
                    {
                        _gridArray[x, y, z] = CreateGridObject(this, new Vector3Int(x, y, z));
                    }
                }
            }
        }

        private void UpdateDebugText(Vector3Int pos)
        {
            _gridDebugArray[pos.x, pos.y, pos.z].GetComponent<TextMeshPro>()
                .SetText($"x: {pos.x}\ny: {pos.y}\nz: {pos.z}\n{_gridArray[pos.x, pos.y, pos.z]?.ToString()}");
        }

        public Vector3 GetWorldPosition(Vector3 position)
        {
            return position * _cellSize + _originPosition;
        }

        public Vector3 GetWorldPosition(int x, int y, int z)
        {
            return GetWorldPosition(new Vector3(x, y, z));
        }

        private Vector3Int GetGridPosition(Vector3 worldPosition)
        {
            Vector3 position = (worldPosition - _originPosition) / _cellSize;
            return Vector3Int.FloorToInt(position);
        }
        
        private bool IsPositionInGrid(Vector3Int pos)
        {
            return pos is {x: >= 0, y: >= 0, z: >= 0} 
                   && pos.x < _gridSize.x && pos.y < _gridSize.y && pos.z < _gridSize.z;
        }

        private void SetGridObjectLocal(Vector3Int pos, T value)
        {
            if (IsPositionInGrid(pos))
            {
                _gridArray[pos.x, pos.y, pos.z] = value;
                TriggerGridObjectChanged(pos);
            }
            else
            {
                Debug.LogWarning("Setting values for grid failed: Out of bounds!");
            }
        }

        public void SetGridObjectWorld(Vector3 worldPosition, T value)
        {
            SetGridObjectLocal(GetGridPosition(worldPosition), value);
        }
        
        public void TriggerGridObjectChanged(Vector3Int pos)
        {
            OnGridValueChanged?.Invoke(pos);
        }
        
        public T GetGridObjectLocal(Vector3Int pos)
        {
            if (IsPositionInGrid(pos))
            {
                return _gridArray[pos.x, pos.y, pos.z];
            }
            else
            {
                Debug.LogWarning("Setting values for grid failed: Out of bounds!");
                return default(T);
            }
        }

        public T GetGridObjectWorld(Vector3 worldPosition)
        {
            return GetGridObjectLocal(GetGridPosition(worldPosition));
        }

        private void DrawDebugLines()
        {
            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    for (int z = 0; z < _gridArray.GetLength(2); z++)
                    {
                        Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y + 1, z), Color.blue, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x + 1, y, z), Color.blue, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y, z + 1), Color.blue, 100f);
                    }
                }
            }
            //Top
            Debug.DrawLine(GetWorldPosition(0, _gridSize.y, 0), GetWorldPosition(_gridSize.x, _gridSize.y, 0), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(0, _gridSize.y, 0), GetWorldPosition(0, _gridSize.y, _gridSize.z), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(_gridSize), GetWorldPosition(0, _gridSize.y, _gridSize.z), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(_gridSize), GetWorldPosition(_gridSize.x, _gridSize.y, 0), Color.blue, 100f);
                
            //Right
            Debug.DrawLine(GetWorldPosition(_gridSize.x, 0, 0), GetWorldPosition(_gridSize.x, _gridSize.y, 0), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(_gridSize.x, 0, 0), GetWorldPosition(_gridSize.x, 0, _gridSize.z), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(_gridSize), GetWorldPosition(_gridSize.x, _gridSize.y, 0), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(_gridSize), GetWorldPosition(_gridSize.x, 0, _gridSize.z), Color.blue, 100f);
                
            //Back
            Debug.DrawLine(GetWorldPosition(0, 0, _gridSize.z), GetWorldPosition(_gridSize.x, 0, _gridSize.z), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(0, 0, _gridSize.z), GetWorldPosition(0, _gridSize.y, _gridSize.z), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(_gridSize), GetWorldPosition(_gridSize.x, 0, _gridSize.z), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(_gridSize), GetWorldPosition(0, _gridSize.y, _gridSize.z), Color.blue, 100f);
        }
        
        private void FillGridWithDebug()
        {
            GameObject gridDebugContainer = new GameObject("GridDebugContainer");
            
            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    for (int z = 0; z < _gridArray.GetLength(2); z++)
                    { 
                        _gridDebugArray[x, y, z] = new GameObject("DebugText - " + $"x:{x} , y: {y} , z: {y}");
                        _gridDebugArray[x, y, z].transform.SetParent(gridDebugContainer.transform);
                        _gridDebugArray[x, y, z].transform.position = GetWorldPosition(x, y, z) + new Vector3(_cellSize,_cellSize,_cellSize) / 2f;
                        TextMeshPro tmpText = _gridDebugArray[x,y,z].AddComponent<TextMeshPro>();
                        tmpText.fontSize = 1.5f;
                        tmpText.alignment = TextAlignmentOptions.Center;
                        tmpText.color = Color.black;
                        TriggerGridObjectChanged(new Vector3Int(x, y, z));
                    }
                }
            }
        }
    }
}
