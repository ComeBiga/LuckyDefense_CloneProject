using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance => mInstance;
    private static GoldManager mInstance = null;

    public int CurrentGoldCount => mCurrentGoldCount;
    public int CurrentDiaCount => mCurrentDiaCount;

    //[SerializeField]
    //private int _startGoldCount = 100;
    //[SerializeField]
    //private int _startDiaCount = 0;

    private int mCurrentGoldCount = 0;
    private int mCurrentDiaCount = 0;

    public void SetCurrentGoldCount(int amount)
    {
        mCurrentGoldCount = amount;

        UIManager.Instance.SetGoldCount(mCurrentGoldCount);
    }

    public void AddCurrentGoldCount(int amount)
    {
        mCurrentGoldCount += amount;

        UIManager.Instance.SetGoldCount(mCurrentGoldCount);
    }

    public void ReduceCurrentGoldCount(int amount)
    {
        mCurrentGoldCount -= amount;

        UIManager.Instance.SetGoldCount(mCurrentGoldCount);
    }

    public void SetCurrentDiaCount(int amount)
    {
        mCurrentDiaCount = amount;

        UIManager.Instance.SetDiaCount(mCurrentDiaCount);
    }

    public void AddCurrentDiaCount(int amount)
    {
        mCurrentDiaCount += amount;

        UIManager.Instance.SetDiaCount(mCurrentDiaCount);
    }

    public void ReduceCurrentDiaCount(int amount)
    {
        mCurrentDiaCount -= amount;

        UIManager.Instance.SetDiaCount(mCurrentDiaCount);
    }


    private void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }
    }
}
