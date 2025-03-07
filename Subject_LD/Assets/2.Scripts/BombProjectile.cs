using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : Projectile
{
    [SerializeField]
    private float _bombRange = 3f;
    [SerializeField]
    private float _damageRate = .8f;

    protected override void takeDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _bombRange, LayerMask.GetMask("Monster"));

        for (int i = 0; i < hits.Length; ++i)
        {
            Collider2D hitCollider = hits[i];

            if (hitCollider.transform.CompareTag("Monster"))
            {
                var targetMonster = hitCollider.GetComponent<Monster>();

                int finalDamage = (int)(mDamage * _damageRate);
                targetMonster.DecreaseHp(finalDamage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _bombRange);
    }
}
