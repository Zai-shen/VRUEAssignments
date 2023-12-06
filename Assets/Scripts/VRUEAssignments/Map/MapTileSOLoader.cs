using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRUEAssignments.Map
{
    public static class MapTileSOLoader
    {
        private static List<MapTileSO> MapTileSos = new ();
        
        private static List<MapTileSO> MapTilesBase = new ();
        private static List<MapTileSO> MapTilesStraightPath = new ();
        private static List<MapTileSO> MapTilesCornerPath = new ();
        
        public static void Init(IEnumerable<MapTileSO> mapTileSos)
        {
            foreach (MapTileSO mpSO in mapTileSos)
            {
                switch (mpSO.MapTType)
                {
                    case MapTileType.PATH:
                        if (IsStraight(mpSO.MobEntry, mpSO.MobExit))
                        {
                            MapTilesStraightPath.Add(mpSO);
                        }
                        else
                        {
                            MapTilesCornerPath.Add(mpSO);
                        }
                        break;
                    case MapTileType.BASE:
                        MapTilesBase.Add(mpSO);
                        break;
                    case MapTileType.EMPTY:
                    default:
                        Debug.LogWarning("MapTileSOLoader: MapTileSO.MapTileType should not be empty!");
                        break;
                }
                MapTileSos.Add(mpSO);
            }
        }

        public static MapTileSO GetRandomBaseSO()
        {
            int range = MapTilesBase.Count;
            return MapTilesBase[Random.Range(0,range)];
        }
        
        public static MapTileSO GetRandomStraightPathSO()
        {
            int range = MapTilesStraightPath.Count;
            return MapTilesStraightPath[Random.Range(0,range)];
        }
        
        public static MapTileSO GetRandomCornerPathSO()
        {
            int range = MapTilesCornerPath.Count;
            return MapTilesCornerPath[Random.Range(0,range)];
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