using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance => mInstance;
    private static BattleSystem mInstance = null;

    public bool runAI = true;

    [SerializeField]
    private WaveSystem _waveSystem;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private Player _AI;

    [Header("Gold")]
    [SerializeField]
    private int _monsterKillRewardGold = 2;
    [SerializeField]
    private int _diaRewardWaveNumber = 5;
    [SerializeField]
    private int _waveEndRewardDia = 5;
    [SerializeField]
    private int _waveEndRewardGold = 20;

    public void RewardMonsterKill()
    {
        _player.Wallet.AddCurrentGoldCount(_monsterKillRewardGold);
        _AI.Wallet.AddCurrentGoldCount(_monsterKillRewardGold);
    }

    private void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }
    }

    private void Start()
    {
        _player.Init(true);
        _AI.Init(false);

        _waveSystem.onNormalWaveEnd += () =>
        {
            _player.Wallet.AddCurrentGoldCount(_waveEndRewardGold);
            _AI.Wallet.AddCurrentGoldCount(_waveEndRewardGold);

            if(_waveSystem.CurrentWaveCount == _diaRewardWaveNumber)
            {
                _player.Wallet.AddCurrentDiaCount(_waveEndRewardDia);
            }
        };

        _waveSystem.onGameOver += () =>
        {
            UIManager.Instance.goGameOverUI.SetActive(true);
        };

        if (runAI)
        {
            _AI.Run();
        }

        // _player.Run();
    }
}
