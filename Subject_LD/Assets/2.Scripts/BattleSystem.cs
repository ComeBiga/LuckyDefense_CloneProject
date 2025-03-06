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
    [SerializeField]
    private int _startGoldCount = 100;
    [SerializeField]
    private int _startSummonHeroPrice = 20;
    [SerializeField]
    private int _summonHeroIncreasePrice = 2;
    [SerializeField]
    private int _monsterKillRewardGold = 2;
    [SerializeField]
    private int _waveEndRewardGold = 20;

    private int mCurrentGoldCount = 0;
    private int mCurrentSummonHeroPrice;

    public void SummonRandomHeroWithGold()
    {
        if(mCurrentSummonHeroPrice > mCurrentGoldCount)
        {
            Debug.LogError($"골드가 부족합니다!");
            return;
        }

        SummonRandomHero();

        reduceCurrentGoldCount(mCurrentSummonHeroPrice);
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

        summonHero(UnityEngine.Random.Range(100, 101), _summonPointManager.SelectedSummonPoint);
    }

    public void SellHero()
    {
        SummonPoint selectedSummonPoint = _summonPointManager.SelectedSummonPoint;
        selectedSummonPoint.TryGetHero(out Hero hero);
        selectedSummonPoint.RemoveHero(hero);

        _summonPointManager.UnSelect();

        addCurrentGoldCount(_waveSystem.CurrentWaveCount);

        Destroy(hero.gameObject);
    }

    public void RewardMonsterKill()
    {
        addCurrentGoldCount(_monsterKillRewardGold);
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
        setCurrentGoldCount(_startGoldCount);
        setCurrentSummonHeroPrice(_startSummonHeroPrice);

        _waveSystem.onNormalWaveEnd += () =>
        {
            addCurrentGoldCount(_waveEndRewardGold);
        };
    }

    private void summonHero(int heroID, SummonPoint summonPoint)
    {
        Hero targetPrefab = _heroPrefabs.Find(heroPrefab => heroPrefab.ID == heroID);
        Hero summonedHero = Instantiate(targetPrefab);
        summonPoint.AddHero(summonedHero);

        var summonedHeroAttack = summonedHero.GetComponent<HeroAttack>();
        summonedHeroAttack.StartAttack();
    }

    private void setCurrentGoldCount(int amount)
    {
        mCurrentGoldCount = amount;

        UIManager.Instance.SetGoldCount(mCurrentGoldCount);
    }

    private void addCurrentGoldCount(int amount)
    {
        mCurrentGoldCount += amount;

        UIManager.Instance.SetGoldCount(mCurrentGoldCount);
    }

    private void reduceCurrentGoldCount(int amount)
    {
        mCurrentGoldCount -= amount;

        UIManager.Instance.SetGoldCount(mCurrentGoldCount);
    }

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
