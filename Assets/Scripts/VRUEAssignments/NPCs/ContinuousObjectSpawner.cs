using System.Collections.Generic;
using UnityEngine;

namespace VRUEAssignments.NPCs
{
    public class ContinuousObjectSpawner: MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject enemyContainer;
        private float time;
        public float spawnInterval;
        private List<GameObject> enemies;
        private int counter;
        void Start()
        {
            enemies = new List<GameObject> ();
            foreach ( Transform enemyTransform in enemyContainer.transform)
            {
                enemies.Add(enemyTransform.gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
            time += Time.deltaTime;
            if (time >= spawnInterval)
            {
                if (enemies.Count > 0)
                {
                    counter = counter % enemies.Count;
                    Debug.Log("Currently there are " + enemies.Count + " enemies available, counter is " + counter);

                    if (enemies.Count != 0)
                    {
                        var enemy = enemies[counter];
                        var newObject = Instantiate(enemy, transform.position + transform.forward.normalized * 0.1f,
                            Quaternion.identity);
                        //newObject.transform.parent = transform;
                    }

                    time = 0;
                    counter++;
                }
            }
            
        }
    }
}
