using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblingSystem : MonoBehaviour
{
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

    public bool GambleHero(Hero.EGrade grade)
    {
        float successProb = GetProbability(grade);

        float randomValue = UnityEngine.Random.Range(0f, 1f);

        if (randomValue < successProb)
        {
            //List<Hero> heroPrefabsByGrade = HeroManager.Instance.GetHeroPrefabsByGrade(grade);

            //int randomIndex = UnityEngine.Random.Range(0, heroPrefabsByGrade.Count);
            //Hero randomHero = heroPrefabsByGrade[randomIndex];

            //HeroManager.Instance.SummonHero(randomHero.ID);

            return true;
        }

        Debug.LogError("도박 실패!");
        return false;
    }

    public int GetPrice(Hero.EGrade grade)
    {
        int price = -1;

        switch (grade)
        {
            case Hero.EGrade.Normal:
            case Hero.EGrade.Rare:
                price = _normalGamblingPrice;
                break;
            case Hero.EGrade.Hero:
                price = _heroGamblingPrice;
                break;
            case Hero.EGrade.Myth:
                price = _mythGamblingPrice;
                break;
        }

        return price;
    }
    
    public float GetProbability(Hero.EGrade grade)
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

        return successProb;
    }

    public bool IsAvailable(Hero.EGrade grade, int currentDiaCount)
    {
        if (currentDiaCount < GetPrice(grade))
        {
            Debug.LogError("다이아 부족!");
            return false;
        }

        return true;
    }

    //public void GambleNormalHero()
    //{
    //    if (mWallet.CurrentDiaCount < _normalGamblingPrice)
    //    {
    //        Debug.LogError("다이아 부족!");
    //        return;
    //    }

    //    mWallet.ReduceCurrentDiaCount(_normalGamblingPrice);

    //    if (!GambleHero(Hero.EGrade.Normal))
    //    {
    //        Debug.LogError("도박 실패!");
    //        return;
    //    }
    //}

    //public void GambleHeroHero()
    //{
    //    if (mWallet.CurrentDiaCount < _normalGamblingPrice)
    //    {
    //        Debug.LogError("다이아 부족!");
    //        return;
    //    }

    //    mWallet.ReduceCurrentDiaCount(_heroGamblingPrice);

    //    if (!GambleHero(Hero.EGrade.Hero))
    //    {
    //        Debug.LogError("도박 실패!");
    //        return;
    //    }
    //}

    //public void GambleMythHero()
    //{
    //    if (mWallet.CurrentDiaCount < _normalGamblingPrice)
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

}
