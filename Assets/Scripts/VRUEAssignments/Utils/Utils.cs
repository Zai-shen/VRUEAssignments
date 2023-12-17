using UnityEngine;

namespace VRUEAssignments.Utils
{
    public static class Utils
    {
        public static void SetAllChildren(GameObject go, bool active)
        {
            Transform parent = go.transform;
            foreach (Transform t in parent)
            {
                t.gameObject.SetActive(active);
            }
        }
    }
}