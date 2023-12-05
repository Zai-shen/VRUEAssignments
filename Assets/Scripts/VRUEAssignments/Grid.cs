using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Grid
{
    private Vector3 _originPosition;
    
    private int _width;
    private int _height;
    private int[,] _gridArray;
    private float _cellSize;

    private Transform[,] _gridDebugArray;
    
    
    public Grid(int width, int height, float cellSize, Vector3 originPosition = default(Vector3))
    {
        _originPosition = originPosition;
            
        _width = width;
        _height = height;
        _gridArray = new int[_width, _height];
        _cellSize = cellSize;
        
        _gridDebugArray = new Transform[_width, _height];
        
        for (int x = 0; x < width; x ++) {
            for (int y = 0; y < height; y++) {
                // Instantiate(debugGridObjectPrefab, GetWorldPosition(x, y), Quaternion.identity);
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = Vector3.one/10f;
                cube.transform.position = GetWorldPosition(x, y) + new Vector3(1,1) * _cellSize / 2;
                cube.name = "Cube - " + $"x:{x} , y: {y}";
                GameObject textGo = new GameObject("DebugText");
                textGo.transform.SetParent(cube.transform,false);
                TextMeshPro tmpText = textGo.AddComponent<TextMeshPro>();
                tmpText.fontSize = 20f;
                tmpText.alignment = TextAlignmentOptions.Center;
                tmpText.SetText($"x:{x} , y: {y}");
                _gridDebugArray[x, y] = cube.transform;
                // inside for x, y
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.blue, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.blue, 100f);
            }
            // after closing the for x, y
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.blue, 100f);
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }
    
    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        worldPosition -= _originPosition;
        x = Mathf.FloorToInt(worldPosition.x / _cellSize);
        y = Mathf.FloorToInt(worldPosition.y / _cellSize);
    }
    
    public void SetValue(int x, int y, int value) {
        if (x >= 0 && y >= 0 && x < _width && y < _height) {
            _gridArray[x, y] = value;
            _gridDebugArray[x, y].GetChild(0).GetComponent<TextMeshPro>().text = value.ToString();
        }
        else
        {
            Debug.LogWarning("Setting values for grid failed: Out of bounds!");
        }
    }
    
    public void SetValue(Vector3 worldPosition, int value) {
        GetXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);
    }
}
