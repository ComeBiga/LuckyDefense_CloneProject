using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Wallet Wallet => mWallet;

    [SerializeField]
    private GamblingSystem _GamblingSystem;
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
    [SerializeField]
    private int _sellHeroGold = 6;

    private int mCurrentSummonHeroPrice;
    private int mSummonHeroIncreasePrice = 2;
    private bool mbPlayable = false;

    // private HeroManager mHeroManager;
    private Wallet mWallet;
    private BehaviourTreeRunner mBTRunner;

    public void Init(bool playable)
    {
        mWallet = new Wallet();

        if (playable)
        {
            SetPlayable();
        }

        _summonPointManager.interactable = playable;

        mCurrentSummonHeroPrice = _startSummonHeroPrice;
        mSummonHeroIncreasePrice = _summonHeroIncreasePrice;

        mWallet.SetCurrentGoldCount(_startGoldCount);
        mWallet.SetCurrentDiaCount(_startDiaCount);

        setCurrentSummonHeroPrice(_startSummonHeroPrice);
    }

    public void SetPlayable()
    {
        mbPlayable = true;

        UIManager.Instance.btnSummonHero.onClick.AddListener(() =>
        {
            SummonHero();
        });

        UIManager.Instance.btnComposeHero.onClick.AddListener(() =>
        {
            ComposeHero();
        });

        UIManager.Instance.btnSellHero.onClick.AddListener(() =>
        {
            SellHero();
        });

        UIManager.Instance.btnComposeMythHero.onClick.AddListener(() =>
        {
            ComposeMythHero();
        });
        
        // 도박
        UIManager.Instance.btnNormalGamble.onClick.AddListener(() =>
        {
            GambleHero(Hero.EGrade.Normal);
        });
        
        UIManager.Instance.btnHeroGamble.onClick.AddListener(() =>
        {
            GambleHero(Hero.EGrade.Hero);
        });
        
        UIManager.Instance.btnMythGamble.onClick.AddListener(() =>
        {
            GambleHero(Hero.EGrade.Myth);
        });

        // 재화 갱신
        mWallet.onGoldChanged += (currentGoldCount) =>
        {
            UIManager.Instance.SetGoldCount(currentGoldCount);
        };

        mWallet.onDiaChanged += (currentDiaCount) =>
        {
            UIManager.Instance.SetDiaCount(currentDiaCount);
        };
    }

    public INode.EState SummonHero()
    {
        if (mCurrentSummonHeroPrice > mWallet.CurrentGoldCount)
        {
            Debug.LogError($"골드가 부족합니다!");
            return INode.EState.Failure;
        }

        // int randomHeroID = UnityEngine.Random.Range(0, 2);
        List<Hero> heroPrefabsByGrade = HeroManager.Instance.GetHeroPrefabsByGrade(Hero.EGrade.Normal);
        Hero randomHero = heroPrefabsByGrade[UnityEngine.Random.Range(0, heroPrefabsByGrade.Count)];
        HeroManager.Instance.SummonHero(randomHero.ID, _summonPointManager);

        mWallet.ReduceCurrentGoldCount(mCurrentSummonHeroPrice);
        addCurrentSummonHeroPrice(_summonHeroIncreasePrice);

        return INode.EState.Success;
    }

    public INode.EState GambleHero(Hero.EGrade grade)
    {
        if(!_GamblingSystem.IsAvailable(grade, mWallet.CurrentDiaCount))
        {
            return INode.EState.Failure;
        }

        mWallet.ReduceCurrentDiaCount(_GamblingSystem.GetPrice(grade));

        if (_GamblingSystem.GambleHero(grade))
        {
            List<Hero> heroPrefabsByGrade = HeroManager.Instance.GetHeroPrefabsByGrade(grade);

            int randomIndex = UnityEngine.Random.Range(0, heroPrefabsByGrade.Count);
            Hero randomHero = heroPrefabsByGrade[randomIndex];

            HeroManager.Instance.SummonHero(randomHero.ID, _summonPointManager);
        }

        return INode.EState.Success;
    }

    public void ComposeHero()
    {
        if (_summonPointManager.SelectedSummonPoint.PositionType != SummonPoint.EPositionType.Tripple)
        {
            return;
        }

        ComposeHero(_summonPointManager.SelectedSummonPoint);
    }
    
    public INode.EState ComposeHeroByAI()
    {
        SummonPoint fullSummonPoint = _summonPointManager.FindFullSummonPoint();

        if (fullSummonPoint == null)
        {
            return INode.EState.Failure;
        }

        ComposeHero(fullSummonPoint);

        return INode.EState.Success;
    }

    public INode.EState ComposeHero(SummonPoint summonPoint)
    {
        var composedHeroes = new List<Hero>(summonPoint.Heroes);
        Hero.EGrade grade = composedHeroes[0].Grade;

        List<Hero> heroPrefabs = new List<Hero>(10);

        switch (grade)
        {
            case Hero.EGrade.Normal:
                heroPrefabs = HeroManager.Instance.GetHeroPrefabsByGrade(Hero.EGrade.Rare);
                break;
            case Hero.EGrade.Rare:
                heroPrefabs = HeroManager.Instance.GetHeroPrefabsByGrade(Hero.EGrade.Hero);
                break;
            case Hero.EGrade.Hero:
                //heroPrefabs = HeroManager.Instance.GetHeroPrefabsByGrade(Hero.EGrade.Myth);
                return INode.EState.Failure;
        }

        summonPoint.Clear();

        for (int i = composedHeroes.Count - 1; i >= 0; i--)
        {
            Destroy(composedHeroes[i].gameObject);
        }

        int randomIndex = UnityEngine.Random.Range(0, heroPrefabs.Count);
        Hero randomHero = heroPrefabs[randomIndex];
        int randomHeroID = randomHero.ID;

        HeroManager.Instance.SummonHero(randomHeroID, _summonPointManager);

        if (mbPlayable)
        {
            _summonPointManager.UnSelect();
        }

        return INode.EState.Success;
    }

    public INode.EState ComposeMythHero()
    {
        List<Hero> mythHeroPrefabs = HeroManager.Instance.GetHeroPrefabsByGrade(Hero.EGrade.Myth);

        foreach(Hero mythHeroPrefab in mythHeroPrefabs)
        {
            var mythComposition = mythHeroPrefab.GetComponent<HeroMythComposition>();

            if (mythComposition.IsComposable(_summonPointManager.SummonPoints, out Dictionary<int, List<SummonPoint>> materialHeroMap))
            {
                // 각 타일에서 재료 영웅 제거, 타일 중 가장 숫자가 적은 곳을 선택
                foreach(var pair in materialHeroMap)
                {
                    List<SummonPoint> summonPoints = pair.Value;
                    SummonPoint targetPoint = summonPoints[0];

                    foreach(var summonPoint in summonPoints)
                    {
                        if(summonPoint.Heroes.Count < targetPoint.Heroes.Count)
                        {
                            targetPoint = summonPoint;
                        }
                    }

                    targetPoint.TryGetHero(out Hero removeHero);
                    targetPoint.RemoveHero(removeHero);

                    HeroManager.Instance.KillHero(removeHero);
                }

                // 신화 영웅 생성
                HeroManager.Instance.SummonHero(mythHeroPrefab.ID, _summonPointManager);

                return INode.EState.Success;
            }
        }

        return INode.EState.Failure;
    }

    public INode.EState SellHero()
    {
        SummonPoint selectedSummonPoint = _summonPointManager.SelectedSummonPoint;
        selectedSummonPoint.TryGetHero(out Hero hero);
        selectedSummonPoint.RemoveHero(hero);

        _summonPointManager.UnSelect();

        mWallet.AddCurrentGoldCount(_sellHeroGold);

        // 판매시 같은 영웅 뭉치기
        for(int i = 0; i < _summonPointManager.SummonPoints.Length; ++i)
        {
            SummonPoint targetSummonPoint = _summonPointManager.SummonPoints[i];

            if(targetSummonPoint != selectedSummonPoint && targetSummonPoint.Contains(hero.ID))
            {
                if(targetSummonPoint.Heroes.Count <= selectedSummonPoint.Heroes.Count)
                {
                    targetSummonPoint.TryGetHero(out Hero targetHero);
                    targetSummonPoint.RemoveHero(targetHero);

                    selectedSummonPoint.AddHero(targetHero);
                }
            }
        }

        HeroManager.Instance.KillHero(hero);

        return INode.EState.Success;
    }


    public void Run()
    {
        mBTRunner = new BehaviourTreeRunner(CreateBehaviourTree());

        StartCoroutine(eRunBT());
    }

    private INode CreateBehaviourTree()
    {
        var selectorNode = new SelectorNode(
            new List<INode>()
            {
                new ActionNode(ComposeHeroByAI),
                new ActionNode(SummonHero),
            });

        var rootNode = selectorNode;

        return rootNode;
    }

    private IEnumerator eRunBT()
    {
        float interval = .5f;
        float timer = 0;

        while (true)
        {
            if (timer > interval)
            {
                timer = 0;
                mBTRunner.Operate();
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void setCurrentSummonHeroPrice(int amount)
    {
        mCurrentSummonHeroPrice = amount;

        if(mbPlayable)
            UIManager.Instance.SetSummonHeroPrice(mCurrentSummonHeroPrice);
    }

    private void addCurrentSummonHeroPrice(int amount)
    {
        mCurrentSummonHeroPrice += amount;

        if(mbPlayable)
            UIManager.Instance.SetSummonHeroPrice(mCurrentSummonHeroPrice);
    }

    private void reduceCurrentSummonHeroPrice(int amount)
    {
        mCurrentSummonHeroPrice -= amount;

        if(mbPlayable)
            UIManager.Instance.SetSummonHeroPrice(mCurrentSummonHeroPrice);
    }
}
