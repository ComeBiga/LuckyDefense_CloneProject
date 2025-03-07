using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Ranger : Projectile
{
    [SerializeField]
    [Range(0f, 1f)]
    private float _magicArrowProb = .12f;
    [SerializeField]
    private float _magicArrowDamageRate = 8f;

    protected override void takeDamage()
    {
        float randomValue = UnityEngine.Random.Range(0f, 1f);

        int finalDamage = mDamage;

        if (randomValue < _magicArrowProb)
        {
            finalDamage = (int)(finalDamage * _magicArrowDamageRate);
        }

        mTargetMonster.DecreaseHp(finalDamage);
    }
}
