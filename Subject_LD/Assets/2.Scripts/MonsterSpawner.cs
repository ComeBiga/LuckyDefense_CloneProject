using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private Monster _enemyPrefab;
    [SerializeField]
    private int _spawnCount = 5;
    [SerializeField]
    private float _spawnInterval;
    [SerializeField]
    private Transform[] _wayPoints;

    private void Start()
    {
        StartCoroutine(eSpawnEnemy());
    }

    private IEnumerator eSpawnEnemy()
    {
        int spawnedCount = 0;

        while(true)
        {
            Monster newMonster = Instantiate<Monster>(_enemyPrefab);

            var monsterMovement = newMonster.GetComponent<MonsterMovement>();
            monsterMovement.SetWayPoints(_wayPoints);

            monsterMovement.StartMove();

            if(++spawnedCount >= _spawnCount)
            {
                break;
            }

            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}
