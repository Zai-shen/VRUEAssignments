using System;
using TMPro;
using UnityEngine;

namespace VRUEAssignments.Map
{
    public class Grid<T>
    {
        public Action<Vector3Int> OnGridValueChanged;

        private Vector3 _originPosition;

        private int _width;
        private int _height;
        private int _depth;
        private T[,,] _gridArray;
        private float _cellSize;

        private bool _debug;
        private GameObject[,,] _gridDebugArray;

        public Grid(Vector3Int size, float cellSize, Func<Grid<T>, int, int, int, T> CreateGridObject,
            Vector3 originPosition = default(Vector3), bool debug = false) : this(size.x, size.y, size.z, cellSize, CreateGridObject, originPosition, debug)
        { }

        public Grid(int width, int height, int depth, float cellSize, Func<Grid<T>, int, int, int, T> CreateGridObject, Vector3 originPosition = default(Vector3), bool debug = false)
        {
            _originPosition = originPosition;
            _width = width;
            _height = height;
            _depth = depth;
            _gridArray = new T[_width, _height,_depth];
            _cellSize = cellSize;
            
            _debug = debug;
            if (_debug)
            {
                _gridDebugArray = new GameObject[_width, _height, _depth];
                FillGridWithDebug();
                DrawDebugLines();
                OnGridValueChanged += UpdateDebugText;
            }

            FillGridWithDefault(CreateGridObject);
        }
        
        private void FillGridWithDefault(Func<Grid<T>, int, int, int, T> CreateGridObject)
        {
            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    for (int z = 0; z < _gridArray.GetLength(2); z++)
                    {
                        _gridArray[x,y,z] = CreateGridObject(this, x, y, z);
                    }
                }
            }
        }

        private void UpdateDebugText(int x, int y, int z)
        {
            _gridDebugArray[x, y, z].GetComponent<TextMeshPro>()
                .SetText($"x: {x}\ny: {y}\nz: {z}\n{_gridArray[x, y, z]?.ToString()}");
        }
        
        private void UpdateDebugText(Vector3Int vec)
        {
            UpdateDebugText(vec.x, vec.y, vec.z);
        }

        private Vector3 GetWorldPosition(int x, int y, int z)
        {
            return new Vector3(x, y, z) * _cellSize + _originPosition;
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y, out int z)
        {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
            z = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellSize);
        }

        public void SetGridObject(int x, int y, int z, T value)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                _gridArray[x, y, z] = value;
                TriggerGridObjectChanged(x, y, z);
            }
            else
            {
                Debug.LogWarning("Setting values for grid failed: Out of bounds!");
            }
        }

        public void TriggerGridObjectChanged(int x, int y, int z)
        {
            OnGridValueChanged?.Invoke(new Vector3Int(x, y, z));
        }

        public void SetGridObject(Vector3 worldPosition, T value)
        {
            GetXY(worldPosition, out int x, out int y, out int z);
            SetGridObject(x, y, z, value);
        }
        
        public T GetGridObject(int x, int y, int z)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                return _gridArray[x, y, z];
            }
            else
            {
                Debug.LogWarning("Setting values for grid failed: Out of bounds!");
                return default(T);
            }
        }

        public T GetGridObject(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y, out int z);
            return GetGridObject(x, y, z);
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
            Debug.DrawLine(GetWorldPosition(0, _height, 0), GetWorldPosition(_width, _height, 0), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(0, _height, 0), GetWorldPosition(0, _height, _depth), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(_width, _height, _depth), GetWorldPosition(0, _height, _depth), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(_width, _height, _depth), GetWorldPosition(_width, _height, 0), Color.blue, 100f);
                
            //Right
            Debug.DrawLine(GetWorldPosition(_width, 0, 0), GetWorldPosition(_width, _height, 0), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(_width, 0, 0), GetWorldPosition(_width, 0, _depth), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(_width, _height, _depth), GetWorldPosition(_width, _height, 0), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(_width, _height, _depth), GetWorldPosition(_width, 0, _depth), Color.blue, 100f);
                
            //Back
            Debug.DrawLine(GetWorldPosition(0, 0, _depth), GetWorldPosition(_width, 0, _depth), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(0, 0, _depth), GetWorldPosition(0, _height, _depth), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(_width, _height, _depth), GetWorldPosition(_width, 0, _depth), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(_width, _height, _depth), GetWorldPosition(0, _height, _depth), Color.blue, 100f);
        }
        
        private void FillGridWithDebug()
        {
            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    for (int z = 0; z < _gridArray.GetLength(2); z++)
                    { 
                        _gridDebugArray[x, y, z] = new GameObject("DebugText - " + $"x:{x} , y: {y} , z: {y}");
                        _gridDebugArray[x, y, z].transform.position = GetWorldPosition(x, y, z) + new Vector3(1, 1) * _cellSize / 2;
                        TextMeshPro tmpText = _gridDebugArray[x,y,z].AddComponent<TextMeshPro>();
                        tmpText.fontSize = 1.5f;
                        tmpText.alignment = TextAlignmentOptions.Center;
                        UpdateDebugText(x, y, z);
                    }
                }
            }
        }
    }
}
