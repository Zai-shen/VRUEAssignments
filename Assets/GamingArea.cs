using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VRUEAssignments
{
    public class GamingArea : MonoBehaviour
    {
        private VRUEAssignments.Grid<MapPart> _gamingAreaGrid;

        private void Start()
        {
            _gamingAreaGrid = new VRUEAssignments.Grid<MapPart>(10, 10, 1,
                (Grid<MapPart> mp, int x, int y) => new MapPart(0, mp, x, y),
                transform.position);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _gamingAreaGrid.GetGridObject(i,j).AddValue(i+j);
                }
            }
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                var worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
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