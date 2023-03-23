using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music
{
    public class GameLoop : Plum.Base.Singleton<GameLoop>
    {
        [SerializeField] private BoxCollider ground;
        [SerializeField] private GameObject enemyPrefab;
        private void Start()
        {
            SpawnEnemy();
        }

        public void SpawnEnemy()
        {
            Instantiate(enemyPrefab, BoundPoint(), Quaternion.identity);
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
