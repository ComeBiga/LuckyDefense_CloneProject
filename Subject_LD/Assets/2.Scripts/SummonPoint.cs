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

    [SerializeField]
    private Transform[] _doublePositions;
    [SerializeField]
    private Transform[] _tripplePositions;

    private List<Hero> mHeroes = new List<Hero>(3);
    private EPositionType mPositionType = EPositionType.None;

    public void Clear()
    {
        mHeroes.Clear();
        refreshPositionType();
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

        refreshPositionType();
        setPosition(mPositionType);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }

    private void refreshPositionType()
    {
        mPositionType = (EPositionType)mHeroes.Count;
    }

    private void setPosition(EPositionType positionType)
    {
        switch(positionType)
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
