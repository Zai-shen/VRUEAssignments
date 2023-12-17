using System;
using Dreamteck.Splines;
using UnityEngine;
using VRUEAssignments.NPCs.Enemies;

public class EnemySpawner : MonoBehaviour
{
    public Transform EnemyContainer;
    public GameObject EnemyPrefab;
    
    public EnemyKind CurrentKind;

    public Action<GameObject> EnemySpawned;
    
    public void Spawn(Vector3 spawnPosition, Quaternion spawnRotation, SplineComputer splineC)
    {
        GameObject enemyGo = Instantiate(EnemyPrefab, spawnPosition, spawnRotation);
        enemyGo.transform.SetParent(EnemyContainer);
        Enemy enemy = enemyGo.GetComponent<Enemy>();
        enemy.ChangeEnemy(CurrentKind);
        enemy.SetSplineToFollow(splineC);
        
        EnemySpawned?.Invoke(enemyGo);
    }
}
