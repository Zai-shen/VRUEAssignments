using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRUEAssignments.Map
{
    public static class MapResourceLoader
    {
        private static List<MapTileSO> _mapTileSos = new ();
        private static readonly List<MapTileSO> _mapTilesBase = new ();
        private static readonly List<MapTileSO> _mapTilesStraightPath = new ();
        private static readonly List<MapTileSO> _mapTilesCornerPath = new ();
        private static readonly List<MapTileSO> _mapTilesTeleport = new ();
        
        public static void Init(IEnumerable<MapTileSO> mapTileSos)
        {
            foreach (MapTileSO mpSo in mapTileSos)
            {
                switch (mpSo.MapTType)
                {
                    case MapTileType.PATH:
                        if (IsStraight(mpSo.MobEntry, mpSo.MobExit))
                        {
                            _mapTilesStraightPath.Add(mpSo);
                        }
                        else
                        {
                            _mapTilesCornerPath.Add(mpSo);
                        }
                        break;
                    case MapTileType.BASE:
                        _mapTilesBase.Add(mpSo);
                        break;
                    case MapTileType.TELEPORT:
                        _mapTilesTeleport.Add(mpSo);
                        break;
                    case MapTileType.EMPTY:
                    default:
                        Debug.LogWarning("MapTileSOLoader: MapTileSO.MapTileType should not be empty!");
                        break;
                }
                _mapTileSos.Add(mpSo);
            }
        }

        public static MapTileSO GetRandomBaseSo()
        {
            int range = _mapTilesBase.Count;
            return _mapTilesBase[Random.Range(0,range)];
        }
        
        public static MapTileSO GetRandomStraightPathSo()
        {
            int range = _mapTilesStraightPath.Count;
            return _mapTilesStraightPath[Random.Range(0,range)];
        }
        
        public static MapTileSO GetRandomCornerPathSo()
        {
            int range = _mapTilesCornerPath.Count;
            return _mapTilesCornerPath[Random.Range(0,range)];
        }
        
        public static MapTileSO GetRandomTeleportSo()
        {
            int range = _mapTilesTeleport.Count;
            return _mapTilesTeleport[Random.Range(0,range)];
        }
        
        private static bool IsStraight(XZCoords entry, XZCoords exit)
        {
            return (entry == XZCoords.UP && exit == XZCoords.DOWN)
                   || (entry == XZCoords.DOWN && exit == XZCoords.UP)
                   || (entry == XZCoords.LEFT && exit == XZCoords.RIGHT)
                   || (entry == XZCoords.RIGHT && exit == XZCoords.LEFT);
        }
    }
}