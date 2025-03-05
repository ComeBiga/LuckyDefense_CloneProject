using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private int _maxHp = 10;

    private int mCurrentHp = 0;
    private MonsterAnimation mMonsterAnimation;

    public void DecreaseHp(int amount)
    {
        mCurrentHp -= amount;

        mMonsterAnimation.BeHit();

        if(mCurrentHp <= 0) 
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    private void Start()
    {
        mMonsterAnimation = GetComponent<MonsterAnimation>();

        mCurrentHp = _maxHp;
    }
}
