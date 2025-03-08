using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRangeAttack : HeroAttack
{
    [SerializeField]
    private Projectile _projectilePrefab;

    private Monster mTargetMonster;

    protected override void attack(Monster targetMonster)
    {
        mTargetMonster = targetMonster;

        mHeroAnimation.Attack();
    }

    protected override void onAttackAnimationEvent()
    {
        Projectile newProjectile = Instantiate(_projectilePrefab);
        newProjectile.transform.position = transform.position;

        newProjectile.Shoot(mTargetMonster, _attackDamage);
    }
}
