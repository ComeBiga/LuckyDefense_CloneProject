using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile : MonoBehaviour
{
    protected Monster mTargetMonster;
    protected int mDamage;

    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float _destroyDistance = .5f;

    private bool mbActive = true;

    public void Shoot(Monster targetMonster, int damage)
    {
        if (targetMonster == null)
        {
            Destroy(this.gameObject);
            return;
        }

        mTargetMonster = targetMonster;
        mDamage = damage;
        mbActive = true;

        mTargetMonster.onDied += () =>
        {
            mbActive = false;
        };

        StartCoroutine(eMove());
    }

    protected virtual void takeDamage()
    {
        mTargetMonster.DecreaseHp(mDamage);
    }

    private IEnumerator eMove()
    {
        while(mbActive)
        {
            if(mTargetMonster == null)
            {
                break;
            }

            // 타겟 방향 계산
            Vector3 direction = (mTargetMonster.transform.position - transform.position).normalized;

            // 이동
            transform.position += direction * _speed * Time.deltaTime;

            // 타겟에 도달했는지 체크
            if (Vector3.Distance(transform.position, mTargetMonster.transform.position) <= _destroyDistance)
            {
                takeDamage();

                break;
            }

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
