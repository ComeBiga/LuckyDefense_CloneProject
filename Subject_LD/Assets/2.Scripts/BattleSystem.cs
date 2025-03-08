using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance => mInstance;
    private static BattleSystem mInstance = null;

    [SerializeField]
    private List<Hero> _heroPrefabs;
    [SerializeField]
    private WaveSystem _waveSystem;
    [SerializeField]
    private SummonPointManager _summonPointManager;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private Player _AI;
    [Header("Gold")]
    // [SerializeField]
    //private int _startGoldCount = 100;
    //[SerializeField]
    //private int _startDiaCount = 0;
    //[SerializeField]
    //private int _startSummonHeroPrice = 20;
    //[SerializeField]
    //private int _summonHeroIncreasePrice = 2;
    [SerializeField]
    private int _monsterKillRewardGold = 2;
    [SerializeField]
    private int _waveEndRewardGold = 20;
    //[Header("Gambling")]
    //[SerializeField]
    //private int _normalGamblingPrice = 1;
    //[SerializeField]
    //private int _heroGamblingPrice = 1;
    //[SerializeField]
    //private int _mythGamblingPrice = 2;
    //[SerializeField]
    //[Range(0f, 1f)]
    //private float _normalGamblingProb = .6f;
    //[SerializeField]
    //[Range(0f, 1f)]
    //private float _heroGamblingProb = .2f;
    //[SerializeField]
    //[Range(0f, 1f)]
    //private float _mythGamblingProb = .1f;

    // private int mCurrentGoldCount = 0;
    // private int mCurrentSummonHeroPrice;

    // private Wallet mWallet;

    //public void SummonRandomHeroWithGold()
    //{
    //    if(mCurrentSummonHeroPrice > mWallet.CurrentGoldCount)
    //    {
    //        Debug.LogError($"골드가 부족합니다!");
    //        return;
    //    }

    //    SummonRandomHero();

    //    mWallet.ReduceCurrentGoldCount(mCurrentSummonHeroPrice);
    //    addCurrentSummonHeroPrice(_summonHeroIncreasePrice);
    //}

    //public bool TrySummonHero(int heroID)
    //{
    //    SummonPoint targetSummonPoint = _summonPointManager.FindSummonPointWithoutFull(heroID);

    //    if(targetSummonPoint == null || targetSummonPoint.IsFull)
    //    {
    //        targetSummonPoint = _summonPointManager.GetEmptySummonPoint();
    //    }

    //    summonHero(heroID, targetSummonPoint);

    //    return true;
    //}

    //public void SummonRandomHero()
    //{
    //    TrySummonHero(UnityEngine.Random.Range(0, 2));
    //}

    //public bool GambleHero(Hero.EGrade grade)
    //{
    //    float successProb = 0f;

    //    switch (grade)
    //    {
    //        case Hero.EGrade.Normal:
    //        case Hero.EGrade.Rare:
    //            successProb = _normalGamblingProb;
    //            break;
    //        case Hero.EGrade.Hero:
    //            successProb = _heroGamblingProb;
    //            break;
    //        case Hero.EGrade.Myth:
    //            successProb = _mythGamblingProb;
    //            break;
    //    }

    //    float randomValue = UnityEngine.Random.Range(0f, 1f);

    //    if(randomValue < successProb)
    //    {
    //        List<Hero> heroPrefabsByGrade = _heroPrefabs.FindAll(hero => hero.Grade == grade);

    //        int randomIndex = UnityEngine.Random.Range(0, heroPrefabsByGrade.Count);
    //        Hero randomHero = heroPrefabsByGrade[randomIndex];

    //        TrySummonHero(randomHero.ID);

    //        return true;
    //    }

    //    return false;
    //}

    //public void GambleNormalHero()
    //{
    //    if (mWallet.CurrentDiaCount < _normalGamblingPrice)
    //    {
    //        Debug.LogError("다이아 부족!");
    //        return;
    //    }

    //    mWallet.ReduceCurrentDiaCount(_normalGamblingPrice);

    //    if(!GambleHero(Hero.EGrade.Normal))
    //    {
    //        Debug.LogError("도박 실패!");
    //        return;
    //    }
    //}

    //public void GambleHeroHero()
    //{
    //    if(mWallet.CurrentDiaCount < _normalGamblingPrice)
    //    {
    //        Debug.LogError("다이아 부족!");
    //        return;
    //    }

    //    mWallet.ReduceCurrentDiaCount(_heroGamblingPrice);

    //    if(!GambleHero(Hero.EGrade.Hero))
    //    {
    //        Debug.LogError("도박 실패!");
    //        return;
    //    }
    //}

    //public void GambleMythHero()
    //{
    //    if(mWallet.CurrentDiaCount < _normalGamblingPrice)
    //    {
    //        Debug.LogError("다이아 부족!");
    //        return;
    //    }

    //    mWallet.ReduceCurrentDiaCount(_mythGamblingPrice);

    //    if (!GambleHero(Hero.EGrade.Myth))
    //    {
    //        Debug.LogError("도박 실패!");
    //        return;
    //    }
    //}

    //public void ComposeHero()
    //{
    //    if(_summonPointManager.SelectedSummonPoint.PositionType != SummonPoint.EPositionType.Tripple)
    //    {
    //        return;
    //    }

    //    var composedHeroes = new List<Hero>(_summonPointManager.SelectedSummonPoint.Heroes);
    //    _summonPointManager.SelectedSummonPoint.Clear();

    //    Hero.EGrade grade = composedHeroes[0].Grade;

    //    for(int i = composedHeroes.Count - 1; i >= 0; i--)
    //    {
    //        Destroy(composedHeroes[i].gameObject);
    //    }

    //    int randomHeroID = 0;

    //    switch(grade)
    //    {
    //        case Hero.EGrade.Normal:
    //            randomHeroID = UnityEngine.Random.Range(100, 101);
    //            break;
    //        case Hero.EGrade.Rare:
    //            randomHeroID = UnityEngine.Random.Range(200, 202);
    //            break;
    //        case Hero.EGrade.Hero:
    //            randomHeroID = UnityEngine.Random.Range(300, 302);
    //            break;
    //    }

    //    // summonHero(UnityEngine.Random.Range(100, 101), _summonPointManager.SelectedSummonPoint);
    //    TrySummonHero(randomHeroID);
    //}

    //public void SellHero()
    //{
    //    SummonPoint selectedSummonPoint = _summonPointManager.SelectedSummonPoint;
    //    selectedSummonPoint.TryGetHero(out Hero hero);
    //    selectedSummonPoint.RemoveHero(hero);

    //    _summonPointManager.UnSelect();

    //    mWallet.AddCurrentGoldCount(_waveSystem.CurrentWaveCount);

    //    Destroy(hero.gameObject);
    //}

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
        };

        _AI.Run();
        // _player.Run();
    }
}
