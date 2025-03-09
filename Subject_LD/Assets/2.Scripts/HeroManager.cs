using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    public static HeroManager Instance => mInstance;
    private static HeroManager mInstance = null;

    public List<Hero> SummonedHeroes => mSummonedHeroes;

    public event Action<int> onSummonHero = null;
    public event Action<int> onKillHero = null;

    [SerializeField]
    private List<Hero> _heroPrefabs;

    private Dictionary<Hero.EGrade, List<Hero>> mHeroPrefabsByGradeDic = new Dictionary<Hero.EGrade, List<Hero>>(5);
    private List<Hero> mSummonedHeroes = new List<Hero>(20);

    public List<Hero> GetHeroPrefabsByGrade(Hero.EGrade grade)
    {
        return mHeroPrefabsByGradeDic[grade];
    }

    public void SummonHero(int heroID, SummonPointManager summonPointManager)
    {
        SummonPoint targetSummonPoint = summonPointManager.FindSummonPointWithoutFull(heroID);

        if (targetSummonPoint == null || targetSummonPoint.IsFull)
        {
            targetSummonPoint = summonPointManager.GetEmptySummonPoint();
        }

        SummonHero(heroID, targetSummonPoint);
    }

    public void SummonHero(int heroID, SummonPoint summonPoint)
    {
        Hero targetPrefab = _heroPrefabs.Find(heroPrefab => heroPrefab.ID == heroID);
        SummonHero(targetPrefab, summonPoint);
    }

    public void SummonHero(Hero heroPrefab, SummonPoint summonPoint)
    {
        Hero summonedHero = Instantiate(heroPrefab);
        summonPoint.AddHero(summonedHero);

        var summonedHeroAttack = summonedHero.GetComponent<HeroAttack>();
        summonedHeroAttack.StartAttack();

        mSummonedHeroes.Add(summonedHero);

        onSummonHero?.Invoke(summonedHero.ID);
    }

    public void KillHero(Hero hero)
    {
        onKillHero?.Invoke(hero.ID);

        Destroy(hero.gameObject);
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
        sortHeroPrefabsByGrade();
    }

    private void sortHeroPrefabsByGrade()
    {
        foreach(Hero heroPrefab in _heroPrefabs)
        {
            if(!mHeroPrefabsByGradeDic.TryGetValue(heroPrefab.Grade, out List<Hero> heroesByGrade))
            {
                heroesByGrade = new List<Hero>(10);
                mHeroPrefabsByGradeDic.Add(heroPrefab.Grade, heroesByGrade);
            }

            heroesByGrade.Add(heroPrefab);
        }
    }
}
