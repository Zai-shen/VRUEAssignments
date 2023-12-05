using System;
using TMPro;
using UnityEngine;

namespace VRUEAssignments.Map
{
    public class Grid<T>
    {
        public Action<Vector2Int> OnGridValueChanged;

        private Vector3 _originPosition;

        private int _width;
        private int _height;
        private T[,] _gridArray;
        private float _cellSize;

        private bool _debug;
        private GameObject[,] _gridDebugArray;


        public Grid(int width, int height, float cellSize, Func<Grid<T>, int, int, T> CreateGridObject, Vector3 originPosition = default(Vector3))
        {
            _originPosition = originPosition;

            _width = width;
            _height = height;
            _gridArray = new T[_width, _height];
            _cellSize = cellSize;

            _gridDebugArray = new GameObject[_width, _height];

            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    _gridArray[x, y] = CreateGridObject(this, x, y);
                }
            }

            _debug = true;
            if (_debug)
            {
                for (int x = 0; x < _gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < _gridArray.GetLength(1); y++)
                    {
                        _gridDebugArray[x, y] = new GameObject("DebugText - " + $"x:{x} , y: {y}");
                        _gridDebugArray[x, y].transform.position = GetWorldPosition(x, y) + new Vector3(1, 1) * _cellSize / 2;
                        TextMeshPro tmpText = _gridDebugArray[x, y].AddComponent<TextMeshPro>();
                        tmpText.fontSize = 2f;
                        tmpText.alignment = TextAlignmentOptions.Center;
                        UpdateDebugText(x, y);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.blue, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.blue, 100f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.blue, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.blue, 100f);

                OnGridValueChanged += UpdateDebugText;
            }
        }

        private void UpdateDebugText(int x, int y)
        {
            _gridDebugArray[x, y].GetComponent<TextMeshPro>()
                .SetText($"x:{x} , y: {y} \n{_gridArray[x, y]?.ToString()}");
        }
        
        private void UpdateDebugText(Vector2Int vec)
        {
            UpdateDebugText(vec.x, vec.y);
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * _cellSize + _originPosition;
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
        }

        public void SetGridObject(int x, int y, T value)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                _gridArray[x, y] = value;
                TriggerGridObjectChanged(x,y);
            }
            else
            {
                Debug.LogWarning("Setting values for grid failed: Out of bounds!");
            }
        }

        public void TriggerGridObjectChanged(int x, int y)
        {
            OnGridValueChanged?.Invoke(new Vector2Int(x,y));
        }

        public void SetGridObject(Vector3 worldPosition, T value)
        {
            GetXY(worldPosition, out int x, out int y);
            SetGridObject(x, y, value);
        }
        
        public T GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                return _gridArray[x, y];
            }
            else
            {
                Debug.LogWarning("Setting values for grid failed: Out of bounds!");
                return default(T);
            }
        }

        public T GetGridObject(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetGridObject(x, y);
        }
    }
}
