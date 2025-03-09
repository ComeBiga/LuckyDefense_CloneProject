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
    public event Action onGameOver = null;

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
    [SerializeField]
    private int _monsterHpIncreasePerWave = 500;
    [SerializeField]
    private int _gameOverMonsterCount = 100;
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

    public void StartBossWave(Action<bool> onWaveEnd = null)
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

    private IEnumerator eStartBossWave(Action<bool> onWaveEnd = null)
    {
        Monster bossMonster = spawnMonster(_bossMonsterPrefab, _wayPoints);
        Monster oppositeBossMonster = null;
        StartCoroutine(eSpawnMonster(_bossMonsterPrefab, 
                                    _wayPoints, 
                                    _oppositeSpawnDelay, 
                                    (spawnMonster) => { oppositeBossMonster = spawnMonster; }));

        yield return new WaitUntil(() => oppositeBossMonster != null);

        var bossMonsters = new List<Monster>();
        bossMonsters.Add(bossMonster);
        bossMonsters.Add(oppositeBossMonster);

        bool bWaveClear = false;

        bossMonster.onDied += () =>
        {
            bossMonsters.Remove(bossMonster);
        };

        oppositeBossMonster.onDied += () =>
        {
            bossMonsters.Remove(oppositeBossMonster);
        };

        mWaveTimer = 0f;
        float waveTime = _bossWaveTime;

        while (true)
        {
            UIManager.Instance.SetRemainWaveTime(waveTime - mWaveTimer);

            if (bossMonsters.Count < 1)
            {
                mWaveTimer = 0f;
                UIManager.Instance.SetRemainWaveTime(0f);

                bWaveClear = true;

                break;
            }

            if (mWaveTimer > waveTime)
            {
                mWaveTimer = 0f;
                UIManager.Instance.SetRemainWaveTime(0f);

                mbGameOver = true;
                onGameOver?.Invoke();

                break;
            }

            mWaveTimer += Time.deltaTime;

            yield return null;
        }

        onWaveEnd?.Invoke(bWaveClear);

        onBossWaveEnd?.Invoke();
    }

    private Monster spawnMonster(Monster monsterPrefab, Transform[] wayPoints)
    {
        Monster newMonster = Instantiate<Monster>(monsterPrefab);
        newMonster.AddMaxHp(_monsterHpIncreasePerWave * (mCurrentWaveCount - 1));
        newMonster.onDied += () =>
        {
            mMonsters.Remove(newMonster);
            UIManager.Instance.SetMonsterCount(mMonsters.Count, _gameOverMonsterCount);
            BattleSystem.Instance.RewardMonsterKill();
        };

        var monsterMovement = newMonster.GetComponent<MonsterMovement>();
        monsterMovement.SetWayPoints(wayPoints);
        monsterMovement.StartMove();

        mMonsters.Add(newMonster);
        UIManager.Instance.SetMonsterCount(mMonsters.Count, _gameOverMonsterCount);

        if(mMonsters.Count >= _gameOverMonsterCount)
        {
            mbGameOver = true;
            onGameOver?.Invoke();
        }

        return newMonster;
    }

    private Monster spawnBossMonster()
    {
        return spawnMonster(_bossMonsterPrefab, _wayPoints);
    }

    private IEnumerator eSpawnMonster(Monster monsterPrefab, Transform[] wayPoints, float delay, Action<Monster> onSpawnMonster = null)
    {
        yield return new WaitForSeconds(delay);

        Monster monster = spawnMonster(monsterPrefab, wayPoints);

        onSpawnMonster?.Invoke(monster);
    }
}
