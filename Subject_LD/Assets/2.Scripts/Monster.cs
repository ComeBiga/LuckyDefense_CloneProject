using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : MonoBehaviour
{
    public event Action onDied = null;

    [SerializeField]
    private int _maxHp = 10;

    private int mCurrentHp = 0;
    private MonsterAnimation mMonsterAnimation;
    private MonsterCanvas mMonsterCanvas;

    public void DecreaseHp(int amount)
    {
        mCurrentHp -= amount;

        mMonsterAnimation.BeHit();

        if(mCurrentHp <= 0) 
        {
            mCurrentHp = 0;
            mMonsterCanvas.SetHPBar(mCurrentHp, _maxHp);

            Die();

            return;
        }

        mMonsterCanvas.SetHPBar(mCurrentHp, _maxHp);
    }

    public void Die()
    {
        onDied?.Invoke();

        Destroy(this.gameObject);
    }

    private void Start()
    {
        mMonsterAnimation = GetComponent<MonsterAnimation>();
        mMonsterCanvas = GetComponentInChildren<MonsterCanvas>();

        mCurrentHp = _maxHp;
    }
}
