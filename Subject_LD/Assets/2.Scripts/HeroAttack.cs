using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    public float AttackRange => _attackRange;

    [SerializeField]
    protected int _attackDamage = 4;
    [SerializeField]
    protected float _attackInterval = .5f;
    [SerializeField]
    protected float _attackRange = 3f;

    protected HeroAnimation mHeroAnimation;
    protected AnimationEventReceiver mAnimationEventReceiver;

    private Monster mTargetMonster;

    public void StartAttack()
    {
        StartCoroutine(eSearchTarget());
    }

    protected virtual void attack(Monster targetMonster)
    {
        mTargetMonster = targetMonster;

        mHeroAnimation.Attack();
    }

    protected virtual void onAttackAnimationEvent()
    {
        mTargetMonster.DecreaseHp(_attackDamage);
    }

    private void Awake()
    {
        mHeroAnimation = GetComponent<HeroAnimation>();
        mAnimationEventReceiver = GetComponentInChildren<AnimationEventReceiver>();
    }

    private void Start()
    {
        if(mAnimationEventReceiver != null)
            mAnimationEventReceiver.onAttack += onAttackAnimationEvent;
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
