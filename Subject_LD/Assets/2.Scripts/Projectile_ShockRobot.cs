using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_ShockRobot : Projectile
{
    [SerializeField]
    [Range(0f, 1f)]
    private float _shockProb = .12f;
    [SerializeField]
    private float _shockRange = 3f;
    [SerializeField]
    private float _damageRate = 3f;
    [SerializeField]
    private float _stunDuration = .75f;

    protected override void takeDamage()
    {
        float randomValue = UnityEngine.Random.Range(0f, 1f);

        if (randomValue > _shockProb)
        {
            mTargetMonster.DecreaseHp(mDamage);

            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _shockRange, LayerMask.GetMask("Monster"));

        for (int i = 0; i < hits.Length; ++i)
        {
            Collider2D hitCollider = hits[i];

            if (hitCollider.transform.CompareTag("Monster"))
            {
                var targetMonster = hitCollider.GetComponent<Monster>();

                int finalDamage = (int)(mDamage * _damageRate);
                targetMonster.DecreaseHp(finalDamage);

                // ±âÀý
                targetMonster.Stun(_stunDuration);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _shockRange);
    }
}
