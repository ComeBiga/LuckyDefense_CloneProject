using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPointManager : MonoBehaviour
{
    [SerializeField]
    private Transform _summonPointParent;

    private SummonPoint[] mSummonPoints;

    public SummonPoint FindSummonPoint(int heroID, bool withoutFull = true)
    {
        for(int i = 0; i < mSummonPoints.Length; ++i)
        {
            if(mSummonPoints[i].TryGetHero(out Hero summonPointHero))
            {
                if(heroID == summonPointHero.ID)
                {
                    return mSummonPoints[i];
                }
            }
        }

        return null;
    }
    public SummonPoint FindSummonPointWithoutFull(int heroID)
    {
        for(int i = 0; i < mSummonPoints.Length; ++i)
        {
            if(mSummonPoints[i].IsFull)
            {
                continue;
            }

            if(mSummonPoints[i].TryGetHero(out Hero summonPointHero))
            {
                if(heroID == summonPointHero.ID)
                {
                    return mSummonPoints[i];
                }
            }
        }

        return null;
    }

    public SummonPoint GetEmptySummonPoint()
    {
        for (int i = 0; i < mSummonPoints.Length; ++i)
        {
            if (!mSummonPoints[i].TryGetHero(out Hero summonPointHero))
            {
                return mSummonPoints[i];
            }
        }

        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        mSummonPoints = findSummonPoints();
    }

    private SummonPoint[] findSummonPoints()
    {
        SummonPoint[] summonPoints = _summonPointParent.GetComponentsInChildren<SummonPoint>();

        return summonPoints;
    }
}
