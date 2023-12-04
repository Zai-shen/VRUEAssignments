using UnityEngine;
using Random = UnityEngine.Random;

namespace VRUEAssignments.Utils
{
    public class SpawnObjectInSphere : MonoBehaviour
    {
        public GameObject ObjectPrefab;
        public float SphereSize = 0.5f;

        private GameObject _spawnedGO;
    
        private void Start()
        {
            _spawnedGO = Instantiate(ObjectPrefab, transform);
            SetRandomPosition();
        }

        [ContextMenu("SetRandomPosition")]
        public void SetRandomPosition()
        {
            _spawnedGO.transform.position = transform.position + RandomPointOnSphere(SphereSize);
        }
    
        private Vector3 RandomPointOnSphere(float radius)
        {
            return Random.insideUnitSphere.normalized * radius;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, SphereSize);
        }
    }
}
