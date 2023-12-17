using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

namespace VRUEAssignments.NPCs.Enemies
{
    public class WaveManager : MonoBehaviour
    {
        public EnemySpawner AEnemySpawner;
        public SplineComputer SplineToFollow;
        public List<GameObject> InstantiatedEnemies = new();
        
        public Action WaveSpawned;

        [SerializeField] private EnemyKind _enemyKind = EnemyKind.DRAGON;
        
        [Range(0.1f,10f)]
        [SerializeField]
        private float _spawnDelay = 0.5f;

        [Range(1,100)]
        [SerializeField] private int _currentWaveSize;

        private void OnEnable()
        {
            AEnemySpawner.EnemySpawned += OnEnemySpawned;
        }
        
        private void OnDisable()
        {
            AEnemySpawner.EnemySpawned -= OnEnemySpawned;
        }

        private void Start()
        {
            SpawnWave();
        }

        public void SpawnWave()
        {
            AEnemySpawner.CurrentKind = _enemyKind;
            StartCoroutine(Spawn(SplineToFollow, _currentWaveSize));
        }

        private void CleanUp()
        {
            if (InstantiatedEnemies != null && InstantiatedEnemies.Count != 0)
            {
                for (int index = 0; index < InstantiatedEnemies.Count; index++)
                {
                    Destroy(InstantiatedEnemies[index]);
                }

                InstantiatedEnemies.Clear();
            }
        }

        private IEnumerator Spawn(SplineComputer splineComputer, int amount)
        {
            SplinePoint[] points = splineComputer.GetPoints();
            Vector3 spawnPosition = points[0].position;
            Quaternion spawnRotation = Quaternion.Euler(0,0,0);
            
            for (int i = 0; i < amount; i++)
            {
                AEnemySpawner.Spawn(spawnPosition, spawnRotation, splineComputer);
                yield return new WaitForSeconds(_spawnDelay);
            }

            WaveSpawned?.Invoke();
        }
        
        private void OnEnemySpawned(GameObject obj)
        {
            AddEnemyToList(obj);
        }

        private void AddEnemyToList(GameObject obj)
        {
            InstantiatedEnemies.Add(obj);
        }
    }
}