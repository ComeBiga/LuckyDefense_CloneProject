using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    public int CurrentWaveCount => mCurrentWaveCount;
    public float RemainWaveTime => _waveTime - mWaveTimer;

    public event Action onNormalWaveEnd = null;
    public event Action onBossWaveEnd = null;

    [SerializeField]
    private Monster[] _monsterPrefabs;
    [SerializeField]
    private Monster _bossMonsterPrefab;
    [Header("Wave")]
    [SerializeField]
    private float _waveTime = 20f;
    [SerializeField]
    private float _bossWaveTime = 60f;
    [SerializeField]
    private float _waveInterval = 5f;
    [SerializeField]
    private int _bossWaveNumber = 10;
    [Header("Spawn")]
    [SerializeField]
    private float _spawnInterval;
    [SerializeField]
    private float _oppositeSpawnDelay = .3f;
    [Header("WayPoint")]
    [SerializeField]
    private Transform[] _wayPoints;
    [SerializeField]
    private Transform[] _oppositeWayPoints;

    private float mWaveTimer;
    private int mCurrentWaveCount = 0;
    private bool mbGameOver = false;
    private List<Monster> mMonsters = new List<Monster>(150);

    public void StartWave()
    {
        StartWaveOnce(onWaveEnd: () => 
                    {
                        ++mCurrentWaveCount;
                        UIManager.Instance.SetCurrentWave(mCurrentWaveCount);

                        if (mCurrentWaveCount >= _bossWaveNumber)
                        {
                            StartBossWave();
                        }
                        else
                        {
                            StartWave();
                        }
                    });
    }

    public void StartWaveOnce(Action onWaveEnd = null)
    {
        StartCoroutine(eStartWaveOnce(onWaveEnd));
    }

    public void StartBossWave(Action onWaveEnd = null)
    {
        StartCoroutine(eStartBossWave(onWaveEnd));
    }

    private void Start()
    {
        // StartWaveOnce();
        mCurrentWaveCount = 1;
        UIManager.Instance.SetCurrentWave(mCurrentWaveCount);
        StartWave();
    }

    private IEnumerator eStartWaveOnce(Action onWaveEnd = null)
    {
        mWaveTimer = 0f;
        float waveTime = _waveTime;
        float spawnTimer = _spawnInterval;

        while (true)
        {
            UIManager.Instance.SetRemainWaveTime(waveTime - mWaveTimer);

            if (mWaveTimer > _waveTime)
            {
                mWaveTimer = 0f;
                UIManager.Instance.SetRemainWaveTime(0f);
                UIManager.Instance.SetNextWaveTimer(false, 0f);

                break;
            }

            if (RemainWaveTime > _waveInterval)
            {
                if (spawnTimer > _spawnInterval)
                {
                    spawnTimer = 0f;

                    spawnMonster(_monsterPrefabs[0], _wayPoints);
                    StartCoroutine(eSpawnMonster(_monsterPrefabs[0], _oppositeWayPoints, _oppositeSpawnDelay));
                }
            }
            else
            {
                UIManager.Instance.SetNextWaveTimer(true, waveTime - mWaveTimer);
            }

            mWaveTimer += Time.deltaTime;
            spawnTimer += Time.deltaTime;

            yield return null;
        }

        onWaveEnd?.Invoke();

        onNormalWaveEnd?.Invoke();
    }

    private IEnumerator eStartBossWave(Action onWaveEnd = null)
    {
        Monster bossMonster = spawnBossMonster();

        bool bWaveClear = false;
        bossMonster.onDied += () =>
        {
            bWaveClear = true;
        };

        mWaveTimer = 0f;
        float waveTime = _bossWaveTime;

        while (true)
        {
            UIManager.Instance.SetRemainWaveTime(waveTime - mWaveTimer);

            if (mWaveTimer > waveTime || bWaveClear)
            {
                mWaveTimer = 0f;
                UIManager.Instance.SetRemainWaveTime(0f);

                if (!bossMonster.IsDied)
                {
                    
                }

                break;
            }

            mWaveTimer += Time.deltaTime;

            yield return null;
        }

        onWaveEnd?.Invoke();

        onBossWaveEnd?.Invoke();
    }

    private Monster spawnMonster(Monster monsterPrefab, Transform[] wayPoints)
    {
        Monster newMonster = Instantiate<Monster>(monsterPrefab);
        newMonster.onDied += () =>
        {
            mMonsters.Remove(newMonster);
            UIManager.Instance.SetMonsterCount(mMonsters.Count);
            BattleSystem.Instance.RewardMonsterKill();
        };

        var monsterMovement = newMonster.GetComponent<MonsterMovement>();
        monsterMovement.SetWayPoints(wayPoints);
        monsterMovement.StartMove();

        mMonsters.Add(newMonster);
        UIManager.Instance.SetMonsterCount(mMonsters.Count);

        if(mMonsters.Count >= 100)
        {
            mbGameOver = true;
        }

        return newMonster;
    }

    private Monster spawnBossMonster()
    {
        return spawnMonster(_bossMonsterPrefab, _wayPoints);
    }

    private IEnumerator eSpawnMonster(Monster monsterPrefab, Transform[] wayPoints, float delay)
    {
        yield return new WaitForSeconds(delay);

        spawnMonster(monsterPrefab, wayPoints);
    }
}
