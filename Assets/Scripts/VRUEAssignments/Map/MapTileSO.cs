using UnityEngine;

namespace VRUEAssignments.Map
{
    [CreateAssetMenu(fileName = "MapTileSO", menuName = "VRUEAssignment/ScriptableOjects/MapTileSO", order = 1)]
    public class MapTileSO : ScriptableObject
    {
        public string Name;
        public GameObject TilePrefab;
        
        public MapTileType MapTType;
        public XZCoords MobEntry;
        public XZCoords MobExit;

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}