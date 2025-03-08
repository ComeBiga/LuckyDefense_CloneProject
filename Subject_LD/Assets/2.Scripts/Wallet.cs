using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int CurrentGoldCount => mCurrentGoldCount;
    public int CurrentDiaCount => mCurrentDiaCount;

    public event Action<int> onGoldChanged = null;
    public event Action<int> onDiaChanged = null;

    //[SerializeField]
    //private int _startGoldCount = 100;
    //[SerializeField]
    //private int _startDiaCount = 0;

    private int mCurrentGoldCount = 0;
    private int mCurrentDiaCount = 0;

    public void SetCurrentGoldCount(int amount)
    {
        mCurrentGoldCount = amount;

        onGoldChanged?.Invoke(mCurrentGoldCount);
        // UIManager.Instance.SetGoldCount(mCurrentGoldCount);
    }

    public void AddCurrentGoldCount(int amount)
    {
        mCurrentGoldCount += amount;

        onGoldChanged?.Invoke(mCurrentGoldCount);
        //UIManager.Instance.SetGoldCount(mCurrentGoldCount);
    }

    public void ReduceCurrentGoldCount(int amount)
    {
        mCurrentGoldCount -= amount;

        onGoldChanged?.Invoke(mCurrentGoldCount);
        //UIManager.Instance.SetGoldCount(mCurrentGoldCount);
    }

    public void SetCurrentDiaCount(int amount)
    {
        mCurrentDiaCount = amount;

        onDiaChanged?.Invoke(mCurrentDiaCount);
        //UIManager.Instance.SetDiaCount(mCurrentDiaCount);
    }

    public void AddCurrentDiaCount(int amount)
    {
        mCurrentDiaCount += amount;

        onDiaChanged?.Invoke(mCurrentDiaCount);
        //UIManager.Instance.SetDiaCount(mCurrentDiaCount);
    }

    public void ReduceCurrentDiaCount(int amount)
    {
        mCurrentDiaCount -= amount;

        onDiaChanged?.Invoke(mCurrentDiaCount);
        //UIManager.Instance.SetDiaCount(mCurrentDiaCount);
    }
}
