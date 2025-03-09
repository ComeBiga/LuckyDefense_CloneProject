using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : MonoBehaviour
{
    public bool IsDied => mbIsDied;
    public event Action onDied = null;

    [SerializeField]
    private int _maxHp = 10;

    private int mCurrentHp = 0;
    private bool mbIsDied = false;
    private MonsterMovement mMonsterMovement;
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

    public void Stun(float duration)
    {
        StartCoroutine(eStun(duration));
    }

    public void Die()
    {
        mbIsDied = true;
        onDied?.Invoke();

        Destroy(this.gameObject);
    }

    private void Start()
    {
        mMonsterMovement = GetComponent<MonsterMovement>();
        mMonsterAnimation = GetComponent<MonsterAnimation>();
        mMonsterCanvas = GetComponentInChildren<MonsterCanvas>();

        mCurrentHp = _maxHp;
    }

    private IEnumerator eStun(float duration)
    {
        mMonsterMovement.enable = false;
        mMonsterAnimation.Stun();

        yield return new WaitForSeconds(duration);

        mMonsterMovement.enable = true;
        mMonsterAnimation.Walk();
    }
}
