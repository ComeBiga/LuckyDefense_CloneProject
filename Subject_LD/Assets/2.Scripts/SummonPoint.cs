using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SummonPoint;

public class SummonPoint : MonoBehaviour
{
    public enum EPositionType { None = 0, Single, Double, Tripple }

    public List<Hero> Heroes => mHeroes;
    public EPositionType PositionType => mPositionType;
    public bool IsFull => mPositionType == EPositionType.Tripple;
    public bool IsEmpty => mPositionType == EPositionType.None;

    public event Action<Hero, EPositionType> onAddHero = null;

    [SerializeField]
    private Transform[] _doublePositions;
    [SerializeField]
    private Transform[] _tripplePositions;
    [SerializeField]
    private GameObject _goHoldingSign;
    [SerializeField]
    private GameObject _goSelectedSign;
    [SerializeField]
    private GameObject _goGoalSign;
    [Header("Grade")]
    [SerializeField]
    private SpriteRenderer _srGrade;
    [SerializeField]
    private Color _colorNormal;
    [SerializeField]
    private Color _colorRare;
    [SerializeField]
    private Color _colorHero;
    [SerializeField]
    private Color _colorMyth;

    private List<Hero> mHeroes = new List<Hero>(3);
    private EPositionType mPositionType = EPositionType.None;

    public void Clear()
    {
        mHeroes.Clear();
        refreshGrade();
        refreshPositionType();
    }

    public void Refresh()
    {
        refreshGrade();
        refreshPositionType();
        setPosition(mPositionType);
    }

    public bool Contains(int heroID)
    {
        if(!TryGetHero(out Hero hero))
        {
            return false;
        }

        return heroID == hero.ID;
    }

    public bool TryGetHero(out Hero hero)
    {
        if(mHeroes.Count == 0)
        {
            hero = null;
            return false;
        }

        hero = mHeroes[0];
        return true;
    }

    public void AddHero(Hero hero)
    {
        mHeroes.Add(hero);

        Refresh();

        onAddHero?.Invoke(hero, mPositionType);
    }

    public void AddHeroes(List<Hero> heroes)
    {
        foreach(Hero hero in heroes)
        {
            AddHero(hero);
        }
    }

    public void RemoveHero(Hero hero)
    {
        mHeroes.Remove(hero);

        Refresh();
    }

    public void Hold()
    {
        _goHoldingSign.SetActive(true);
        //_goSelectedSign.SetActive(false);
    }

    public void UnHold()
    {
        _goHoldingSign.SetActive(false);
        //_goSelectedSign.SetActive(false);
    }

    public void Select()
    {
        //_goHoldingSign.SetActive(true);
        _goSelectedSign.SetActive(true);
    }

    public void UnSelect()
    {
        //_goHoldingSign.SetActive(false);
        _goSelectedSign.SetActive(false);
    }

    public void Goal()
    {
        //_goHoldingSign.SetActive(true);
        _goGoalSign.SetActive(true);
    }

    public void UnGoal()
    {
        //_goHoldingSign.SetActive(false);
        _goGoalSign.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }

    private void refreshPositionType()
    {
        if(TryGetHero(out Hero hero) && hero.Grade == Hero.EGrade.Myth)
        {
            mPositionType = EPositionType.Tripple;
            return;
        }

        mPositionType = (EPositionType)mHeroes.Count;
    }

    private void refreshGrade()
    {
        if(!TryGetHero(out Hero hero))
        {
            _srGrade.color = new Color(1f, 1f, 1f, 0f);
            return;
        }

        switch (hero.Grade)
        {
            case Hero.EGrade.Normal:
                _srGrade.color = _colorNormal;
                break;
            case Hero.EGrade.Rare:
                _srGrade.color = _colorRare;
                break;
            case Hero.EGrade.Hero:
                _srGrade.color = _colorHero;
                break;
            case Hero.EGrade.Myth:
                _srGrade.color = _colorMyth;
                break;
        }
    }

    private void setPosition(EPositionType positionType)
    {
        if (TryGetHero(out Hero hero) && hero.Grade == Hero.EGrade.Myth)
        {
            mHeroes[0].transform.position = transform.position;
            return;
        }

        switch (positionType)
        {
            case EPositionType.None:
                break;
            case EPositionType.Single:
                mHeroes[0].transform.position = transform.position;
                break; 
            case EPositionType.Double:
                {
                    for(int i = 0; i < _doublePositions.Length; i++)
                    {
                        mHeroes[i].transform.position = _doublePositions[i].position;
                    }
                }
                break;
            case EPositionType.Tripple:
                {
                    for(int i = 0; i < _tripplePositions.Length; i++)
                    {
                        mHeroes[i].transform.position = _tripplePositions[i].position;
                    }
                }
                break;
        }
    }
}
