using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("Gold")]
    [SerializeField]
    private int _startGoldCount = 100;
    [SerializeField]
    private int _startDiaCount = 0;
    [SerializeField]
    private int _startSummonHeroPrice = 20;
    [SerializeField]
    private int _summonHeroIncreasePrice = 2;
    [SerializeField]
    private int _monsterKillRewardGold = 2;
    [SerializeField]
    private int _waveEndRewardGold = 20;
    [Header("Gambling")]
    [SerializeField]
    private int _normalGamblingPrice = 1;
    [SerializeField]
    private int _heroGamblingPrice = 1;
    [SerializeField]
    private int _mythGamblingPrice = 2;
    [SerializeField]
    [Range(0f, 1f)]
    private float _normalGamblingProb = .6f;
    [SerializeField]
    [Range(0f, 1f)]
    private float _heroGamblingProb = .2f;
    [SerializeField]
    [Range(0f, 1f)]
    private float _mythGamblingProb = .1f;

    // private int mCurrentGoldCount = 0;
    private int mCurrentSummonHeroPrice;

    private GoldManager mGoldManager;

    public void SummonRandomHeroWithGold()
    {
        if(mCurrentSummonHeroPrice > mGoldManager.CurrentGoldCount)
        {
            Debug.LogError($"골드가 부족합니다!");
            return;
        }

        SummonRandomHero();

        mGoldManager.ReduceCurrentGoldCount(mCurrentSummonHeroPrice);
        addCurrentSummonHeroPrice(_summonHeroIncreasePrice);
    }

    public bool TrySummonHero(int heroID)
    {
        SummonPoint targetSummonPoint = _summonPointManager.FindSummonPointWithoutFull(heroID);

        if(targetSummonPoint == null || targetSummonPoint.IsFull)
        {
            targetSummonPoint = _summonPointManager.GetEmptySummonPoint();
        }

        summonHero(heroID, targetSummonPoint);

        return true;
    }

    public void SummonRandomHero()
    {
        TrySummonHero(UnityEngine.Random.Range(0, 2));
    }

    public bool GambleHero(Hero.EGrade grade)
    {
        float successProb = 0f;

        switch (grade)
        {
            case Hero.EGrade.Normal:
            case Hero.EGrade.Rare:
                successProb = _normalGamblingProb;
                break;
            case Hero.EGrade.Hero:
                successProb = _heroGamblingProb;
                break;
            case Hero.EGrade.Myth:
                successProb = _mythGamblingProb;
                break;
        }

        float randomValue = UnityEngine.Random.Range(0f, 1f);

        if(randomValue < successProb)
        {
            List<Hero> heroPrefabsByGrade = _heroPrefabs.FindAll(hero => hero.Grade == grade);

            int randomIndex = UnityEngine.Random.Range(0, heroPrefabsByGrade.Count);
            Hero randomHero = heroPrefabsByGrade[randomIndex];

            TrySummonHero(randomHero.ID);

            return true;
        }

        return false;
    }

    public void GambleNormalHero()
    {
        if (mGoldManager.CurrentDiaCount < _normalGamblingPrice)
        {
            Debug.LogError("다이아 부족!");
            return;
        }

        GoldManager.Instance.ReduceCurrentDiaCount(_normalGamblingPrice);

        if(!GambleHero(Hero.EGrade.Normal))
        {
            Debug.LogError("도박 실패!");
            return;
        }
    }

    public void GambleHeroHero()
    {
        if(mGoldManager.CurrentDiaCount < _normalGamblingPrice)
        {
            Debug.LogError("다이아 부족!");
            return;
        }

        GoldManager.Instance.ReduceCurrentDiaCount(_heroGamblingPrice);

        if(!GambleHero(Hero.EGrade.Hero))
        {
            Debug.LogError("도박 실패!");
            return;
        }
    }

    public void GambleMythHero()
    {
        if(mGoldManager.CurrentDiaCount < _normalGamblingPrice)
        {
            Debug.LogError("다이아 부족!");
            return;
        }

        GoldManager.Instance.ReduceCurrentDiaCount(_mythGamblingPrice);

        if (!GambleHero(Hero.EGrade.Myth))
        {
            Debug.LogError("도박 실패!");
            return;
        }
    }

    public void ComposeHero()
    {
        if(_summonPointManager.SelectedSummonPoint.PositionType != SummonPoint.EPositionType.Tripple)
        {
            return;
        }

        var composedHeroes = new List<Hero>(_summonPointManager.SelectedSummonPoint.Heroes);
        _summonPointManager.SelectedSummonPoint.Clear();

        for(int i = composedHeroes.Count - 1; i >= 0; i--)
        {
            Destroy(composedHeroes[i].gameObject);
        }

        // summonHero(UnityEngine.Random.Range(100, 101), _summonPointManager.SelectedSummonPoint);
        TrySummonHero(UnityEngine.Random.Range(100, 101));
    }

    public void SellHero()
    {
        SummonPoint selectedSummonPoint = _summonPointManager.SelectedSummonPoint;
        selectedSummonPoint.TryGetHero(out Hero hero);
        selectedSummonPoint.RemoveHero(hero);

        _summonPointManager.UnSelect();

        mGoldManager.AddCurrentGoldCount(_waveSystem.CurrentWaveCount);

        Destroy(hero.gameObject);
    }

    public void RewardMonsterKill()
    {
        mGoldManager.AddCurrentGoldCount(_monsterKillRewardGold);
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
        mGoldManager = GoldManager.Instance;

        mGoldManager.SetCurrentGoldCount(_startGoldCount);
        mGoldManager.SetCurrentDiaCount(_startDiaCount);
        setCurrentSummonHeroPrice(_startSummonHeroPrice);

        _waveSystem.onNormalWaveEnd += () =>
        {
            mGoldManager.AddCurrentGoldCount(_waveEndRewardGold);
        };
    }

    private void summonHero(int heroID, SummonPoint summonPoint)
    {
        Hero targetPrefab = _heroPrefabs.Find(heroPrefab => heroPrefab.ID == heroID);
        summonHero(targetPrefab, summonPoint);
        //Hero summonedHero = Instantiate(targetPrefab);
        //summonPoint.AddHero(summonedHero);

        //var summonedHeroAttack = summonedHero.GetComponent<HeroAttack>();
        //summonedHeroAttack.StartAttack();
    }

    private void summonHero(Hero heroPrefab, SummonPoint summonPoint)
    {
        Hero summonedHero = Instantiate(heroPrefab);
        summonPoint.AddHero(summonedHero);

        var summonedHeroAttack = summonedHero.GetComponent<HeroAttack>();
        summonedHeroAttack.StartAttack();
    }

    //private void setCurrentGoldCount(int amount)
    //{
    //    mCurrentGoldCount = amount;

    //    UIManager.Instance.SetGoldCount(mCurrentGoldCount);
    //}

    //private void addCurrentGoldCount(int amount)
    //{
    //    mCurrentGoldCount += amount;

    //    UIManager.Instance.SetGoldCount(mCurrentGoldCount);
    //}

    //private void reduceCurrentGoldCount(int amount)
    //{
    //    mCurrentGoldCount -= amount;

    //    UIManager.Instance.SetGoldCount(mCurrentGoldCount);
    //}

    private void setCurrentSummonHeroPrice(int amount)
    {
        mCurrentSummonHeroPrice = amount;

        UIManager.Instance.SetSummonHeroPrice(mCurrentSummonHeroPrice);
    }

    private void addCurrentSummonHeroPrice(int amount)
    {
        mCurrentSummonHeroPrice += amount;

        UIManager.Instance.SetSummonHeroPrice(mCurrentSummonHeroPrice);
    }

    private void reduceCurrentSummonHeroPrice(int amount)
    {
        mCurrentSummonHeroPrice -= amount;

        UIManager.Instance.SetSummonHeroPrice(mCurrentSummonHeroPrice);
    }
}
