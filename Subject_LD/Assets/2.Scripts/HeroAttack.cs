using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    [SerializeField]
    protected int _attackDamage = 4;
    [SerializeField]
    protected float _attackInterval = .5f;
    [SerializeField]
    protected float _attackRange = 5f;

    protected HeroAnimation mHeroAnimation;

    public void StartAttack()
    {
        StartCoroutine(eSearchTarget());
    }

    protected virtual void attack(Monster targetMonster)
    {
        targetMonster.DecreaseHp(_attackDamage);

        mHeroAnimation.Attack();
    }

    private void Awake()
    {
        mHeroAnimation = GetComponent<HeroAnimation>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }

    private IEnumerator eSearchTarget()
    {
        // Monster lastTargetMonster = null;

        while (true)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _attackRange, LayerMask.GetMask("Monster"));

            if (hits.Length > 0)
            {
                Collider2D randomTarget = hits[Random.Range(0, hits.Length)];

                if(randomTarget.transform.CompareTag("Monster"))
                {
                    var targetMonster = randomTarget.GetComponent<Monster>();

                    attack(targetMonster);

                    // lastTargetMonster = targetMonster;
                }
            }

            yield return new WaitForSeconds(_attackInterval);
        }
    }
}
