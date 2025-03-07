using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRangeAttack : HeroAttack
{
    [SerializeField]
    private Projectile _projectilePrefab;

    protected override void attack(Monster targetMonster)
    {
        Projectile newProjectile = Instantiate(_projectilePrefab);
        newProjectile.transform.position = transform.position;

        newProjectile.Shoot(targetMonster, _attackDamage);

        mHeroAnimation.Attack();
    }
}
