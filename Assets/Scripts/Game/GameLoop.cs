using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music
{
    public class GameLoop : Plum.Base.Singleton<GameLoop>
    {
        private const int maxEnemies = 30;
        [SerializeField] private BoxCollider ground;
        [SerializeField] private GameObject enemyPrefab;
        private float enemytickRate = 5;
        public static int enemyAmount;
        private float enemyTimer;
        private void Start()
        {
            SpawnEnemy();
        }

        private void Update()
        {
            if(enemyTimer > 0)
            {
                enemyTimer -= Time.deltaTime;
            }
            else
            {
                if (enemyAmount < maxEnemies) SpawnEnemy();
                enemyTimer = enemytickRate;
            }
        }

        public void SpawnEnemy()
        {
            Instantiate(enemyPrefab, BoundPoint(), Quaternion.identity);
            enemyAmount++;
        }

        private Vector3 BoundPoint()
        {
            Bounds n = ground.bounds;
            Vector3 point = new Vector3();
            point.x = Random.Range(n.min.x, n.max.x);
            point.y = n.max.y + .1f;
            point.z = Random.Range(n.min.z, n.max.z);
            return point;
        }
    }

}
